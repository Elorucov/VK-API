using ELOR.VKAPILib.Methods;
using ELOR.VKAPILib.Objects;
using ELOR.VKAPILib.Objects.HandlerDatas;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ELOR.VKAPILib {
    public class VKAPI {

        #region Methods

        public GroupsMethods Groups { get; private set; }
        public MessagesMethods Messages { get; private set; }
        public UsersMethods Users { get; private set; }

        #endregion

        #region Properties

        public Func<CaptchaHandlerData, Task<string>> CaptchaHandler { get; set; }
        public Func<string, Task<bool>> ActionConfirmationHandler { get; set; }

        #endregion

        #region Fields

        private int _userId;
        private string _accessToken;
        private string _language;
        private string _domain;
        private string _version = "5.119";

        public int UserId { get { return _userId; } }
        public string AccessToken { get { return _accessToken; } }
        public string Language { get { return _language; } }
        public string Domain { get { return _domain; } }
        public string Version { get { return _version; } }

        public static string UserAgent { get; set; }

        #endregion

        #region Events

        public event EventHandler UserAuthorizationFailed;
        public event EventHandler<Uri> ValidationRequired;
        public event EventHandler UserDeletedOrBanned;

        #endregion

        public VKAPI(int userId, string accessToken, string language, string domain = "api.vk.com") {
            _userId = userId;
            _accessToken = accessToken;
            _language = language;
            _domain = domain;

            Groups = new GroupsMethods(this, "groups");
            Messages = new MessagesMethods(this, "messages");
            Users = new UsersMethods(this, "users");
        }

        private Dictionary<string, string> GetNormalizedParameters(Dictionary<string, string> parameters) {
            Dictionary<string, string> prmkv = new Dictionary<string, string>();

            foreach (var a in parameters) {
                prmkv.Add(a.Key, WebUtility.UrlDecode(a.Value));
            }

            prmkv.Add("access_token", AccessToken);
            prmkv.Add("lang", Language);
            prmkv.Add("v", Version);

            return prmkv;
        }

        private async Task<string> SendRequestAsync(string method, Dictionary<string, string> parameters = null) {
            string requestUri = $@"https://{Domain}/method/{method}";

            HttpClientHandler handler = new HttpClientHandler() {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            using (var httpClient = new HttpClient(handler)) {
                httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };

                httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip,deflate");
                if (!String.IsNullOrEmpty(UserAgent)) httpClient.DefaultRequestHeaders.Add("User-Agent", UserAgent);

                using (HttpRequestMessage hmsg = new HttpRequestMessage(HttpMethod.Post, new Uri(requestUri))) {
                    hmsg.Content = new FormUrlEncodedContent(parameters);
                    using (var resp = await httpClient.SendAsync(hmsg)) {
                        if (resp.IsSuccessStatusCode) {
                            string response = await resp.Content.ReadAsStringAsync();
                            return response;
                        } else {
                            throw new Exception($"API server returns http-error: {(int)resp.StatusCode} {resp.StatusCode}.");
                        }
                    }
                }
            }
        }

        public async Task<T> CallMethodAsync<T>(string method, Dictionary<string, string> parameters = null) {
            if(parameters == null) parameters = new Dictionary<string, string>();

            string response = await SendRequestAsync(method, GetNormalizedParameters(parameters));
            JObject jr = JObject.Parse(response);
            if(jr["error"] != null) {
                APIException apiex = JsonConvert.DeserializeObject<APIException>(jr["error"].ToString(Formatting.None));
                switch(apiex.Code) {
                    case 5: UserAuthorizationFailed?.Invoke(this, null); throw apiex;
                    case 14: return await HandleCaptchaRequest<T>(apiex, method, parameters).ConfigureAwait(false);
                    case 17: ValidationRequired?.Invoke(this, apiex.RedirectUri); throw apiex;
                    case 18: UserDeletedOrBanned?.Invoke(this, null); throw apiex;
                    case 24: return await HandleActionConfirmationRequest<T>(apiex, method, parameters).ConfigureAwait(false);
                    default: throw apiex;
                }
            } else if(jr["response"] != null) {
                return JsonConvert.DeserializeObject<T>(jr["response"].ToString(Formatting.None));
            } else {
                throw new Exception("Invalid response.");
            }
        }

        private async Task<T> HandleCaptchaRequest<T>(APIException apiex, string method, Dictionary<string, string> parameters) {
            if(CaptchaHandler != null) {
                CaptchaHandlerData chd = new CaptchaHandlerData {
                    SID = apiex.CaptchaSID,
                    Image = new Uri(apiex.CaptchaImage)
                };
                string key = String.Empty;
                key = await CaptchaHandler.Invoke(chd);
                if (String.IsNullOrEmpty(key)) throw apiex;
                parameters.Add("captcha_sid", apiex.CaptchaSID);
                parameters.Add("captcha_key", key);
                return await CallMethodAsync<T>(method, parameters).ConfigureAwait(false);
            } else {
                throw apiex;
            }
        }

        private async Task<T> HandleActionConfirmationRequest<T>(APIException apiex, string method, Dictionary<string, string> parameters) {
            if(ActionConfirmationHandler != null) {
                bool result = await ActionConfirmationHandler.Invoke(apiex.ConfirmationText);
                if(!result) throw apiex;
                parameters.Add("confirm", "1");
                return await CallMethodAsync<T>(method, parameters).ConfigureAwait(false);
            } else {
                throw apiex;
            }
        }
    }
}