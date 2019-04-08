using System.Collections.Generic;
using Nancy;
using Nancy.ModelBinding;

namespace LoyaltyProgram {
    public class UsersModule : NancyModule {
        private static IDictionary<int, LoyaltyProgramUser> registeredUsers = new Dictionary<int, LoyaltyProgramUser> ();

        public UsersModule () : base ("/users") {
            Post ("/", _ => {
                var newUser = this.Bind<LoyaltyProgramUser> ();
                this.AddRegisteredUser (newUser);
                return this.CreatedResponse (newUser);
            });

            Put ("/{userId:int}", parameters => {
                int userId = parameters.userId;
                var updatedUser = this.Bind<LoyaltyProgramUser> ();
                // store the updated user to a datastore
                return updatedUser;
            });

            Get ("/{userId:int}", parameters => {
                int userId = parameters.userId;
                if (registeredUsers.ContainsKey (userId))
                    return registeredUsers[userId];
                else
                    return HttpStatusCode.NotFound;
            });
        }

        private dynamic CreatedResponse (LoyaltyProgramUser newUser) {
            return this.Negotiate
                .WithStatusCode (HttpStatusCode.Created)
                .WithHeader ("Location", this.Request.Url.SiteBase + "/uses/" + newUser.Id)
                .WithModel (newUser);
        }

        private void AddRegisteredUser (LoyaltyProgramUser newUser) {
            // store the newUser to a data store
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