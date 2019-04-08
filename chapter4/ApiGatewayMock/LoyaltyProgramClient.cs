using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ApiGatewayMock {
    public class LoyaltyProgramClient {
        public async Task<LoyaltyProgramUser> RegisterUser (LoyaltyProgramUser newUser) {
            using (var httpClient = new HttpClient ()) {
                httpClient.BaseAddress = new Uri ($"http://{this.hostName}");
                var response = await httpClient.PostAsync ("/users/", new StringContent (JsonConvert.SerializeObject (newUser), Encoding.UTF8, "application/json"));
                ThrowOnTransientFailure (response);
                return JsonConvert.DeserializeObject<LoyaltyProgramUser> (await response.Content.ReadAsStringAsync ());
            }
        }

        public async Task<LoyaltyProgramUser> UpdateUser (LoyaltyProgramUser user) {
            using (var httpClient = new HttpClient ()) {
                httpClient.BaseAddress = new Uri ($"http://{this.hostName}");
                var response = await httpClient.PutAsync ($"/users/{user.Id}", new StringContent (JsonConvert.SerializeObject (user), Encoding.UTF8, "application/json"));
                ThrowOnTransientFailure (response);
                return JsonConvert.DeserializeObject<LoyaltyProgramUser> (await response.Content.ReadAsStringAsync ());
            }
        }

        public async Task<LoyaltyProgramUser> QueryUser (int userId) {
            var userResource = $"/users/{userId}";
            using (var httpClient = new HttpClient ()) {
                httpClient.BaseAddress = new Uri ($"http://{this.hostName}");
                var response = await httpClient.GetAsync (userResource);
                ThrowOnTransientFailure (response);
                return JsonConvert.DeserializeObject<LoyaltyProgramUser> (await response.Content.ReadAsStringAsync ());
            }
        }

        private static void ThrowOnTransientFailure (HttpResponseMessage response) {
            if (((int) response.StatusCode) < 200 || ((int) response.StatusCode) > 499) throw new Exception (response.StatusCode.ToString ());
        }
    }

    public class LoyaltyProgramUser {
        public int Id { get; set; }
        public string Name { get; set; }
        public int LoyaltyPoints { get; set; }
        public LoyaltyProgramSettings Settings { get; set; }
    }

    public class LoyaltyProgramSettings {
        public string[] Interests { get; set; }
    }
}