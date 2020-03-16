using ELOR.VKAPILib.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;

namespace ELOR.VKAPILib {
    public class LongPoll {

        public event EventHandler<string> DebugInfoReceived;
        public event EventHandler<int> NeedNewServerInfo;
        public event EventHandler<bool> InternetAvailabilityChanged;
        public event EventHandler<Tuple<Exception, int>> CaughtException;

        private void WriteToDebug(string info) {
            DebugInfoReceived?.Invoke(this, info);
        }

        const int WaitTime = 25;
        const int Mode = 234;

        private LongPollServerInfo Info;
        private VKAPI API;
        private CancellationTokenSource cts = new CancellationTokenSource();
        private bool IsRunning = false;
        private int RetryAfterSeconds = 0;
        private bool IsInternetAvailable { get { return NetworkInformation.GetInternetConnectionProfile() != null; } }

        private HttpClient httpClient = new HttpClient();

        public LongPoll(LongPollServerInfo info, VKAPI api) {
            NetworkInformation.NetworkStatusChanged += NetworkInformation_NetworkStatusChanged;
            SetInfo(info);
            API = api;
        }

        private void NetworkInformation_NetworkStatusChanged(object sender) {
            WriteToDebug($"Is internet available: {IsInternetAvailable}");
            InternetAvailabilityChanged?.Invoke(this, IsInternetAvailable);
            IsRunning = IsInternetAvailable;
            if (!IsInternetAvailable) {
                httpClient.Dispose();
            } else {
                Run();
            }
        }

        private void SetInfo(LongPollServerInfo info) {
            Info = info;
            Run();
        }

        public void Stop() {
            IsRunning = false;
            cts.Cancel();
            NetworkInformation.NetworkStatusChanged -= NetworkInformation_NetworkStatusChanged;
            httpClient.Dispose();
        }

        private async void Run() {
            httpClient = new HttpClient();
            IsRunning = true;
            WriteToDebug($"START! Server: {Info.Server}, TS: {Info.TS}");
            while(IsRunning) {
                try {
                    WriteToDebug($"Waiting {Info.TS}...");
                    object r = await GetStateAsync(new Uri($"https://{Info.Server}?act=a_check&key={Info.Key}&ts={Info.TS}&wait={WaitTime}&mode={Mode}&version=9"), cts.Token).ConfigureAwait(false);
                    if (r is LongPollResponse res) {
                        RetryAfterSeconds = 0;
                        WriteToDebug($"Received {Info.TS}...");
                        Info.TS = res.TS;
                        // ParseLPUpdates(res.Updates);
                    } else if (r is LongPollFail rf) {
                        RetryAfterSeconds = 0;
                        WriteToDebug($"LP FAIL: {rf.FailCode}.");
                        switch (rf.FailCode) {
                            case 1: Info.TS = rf.TS; break;
                            case 2:
                            case 3: Stop(); NeedNewServerInfo?.Invoke(this, rf.FailCode); break;
                        }
                    } else if (r is Exception ex) {
                        throw ex;
                    }
                } catch (Exception ex) {
                    if (!IsInternetAvailable) return;
                    RetryAfterSeconds += 15;
                    WriteToDebug($"EX FAIL: (0x{ex.HResult.ToString("x8")}), retry after {RetryAfterSeconds} sec.");
                    CaughtException?.Invoke(this, new Tuple<Exception, int>(ex, RetryAfterSeconds));
                    await Task.Delay(RetryAfterSeconds * 1000).ConfigureAwait(false);
                }
            }
        }

        public async Task<object> GetStateAsync(Uri longPollUri, CancellationToken ct) {
            try {
                var res = await httpClient.GetAsync(longPollUri);

                if (ct.IsCancellationRequested) {
                    throw new TaskCanceledException();
                };

                res.EnsureSuccessStatusCode();
                string restr = await res.Content.ReadAsStringAsync();
                JObject jr = JObject.Parse(restr);
                if (jr["ts"] != null) {
                    LongPollResponse resp = JsonConvert.DeserializeObject<LongPollResponse>(restr);
                    resp.Raw = restr;
                    res.Dispose();
                    return resp;
                } else if (jr["failed"] != null) {
                    res.Dispose();
                    return JsonConvert.DeserializeObject<LongPollFail>(restr);
                } else {
                    res.Dispose();
                    throw new Exception($"A non-standart response was received:\n{restr}");
                }
            } catch (Exception ex) {
                return ex;
            }
        }
    }
}
