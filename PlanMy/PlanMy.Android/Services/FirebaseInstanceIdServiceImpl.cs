using System;
using Android.App;
using Firebase.Iid;
using System.Net;
using PlanMy.Library;

namespace PlanMy.Droid
{
	[Service]
	[IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
	public class FirebaseInstanceIdServiceImpl : FirebaseInstanceIdService
	{
		public async override void OnTokenRefresh()
		{
			// Get updated InstanceID token.
			var refreshedToken = FirebaseInstanceId.Instance.Token;
            Connect con = new Connect();
            var usr = await con.GetData("User");
            if (!string.IsNullOrEmpty(usr))
            {
                Users cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<Users>(usr);
                WebClient client = new WebClient();
                var oldtoken = await con.GetData("FirebaseToken");
                string updateLink = Statics.apiLink + "AddPushToken?UserId=" + cookie.Id + "&NewToken=" + refreshedToken+"&OldToken="+oldtoken+"&PushDevice="+ PushDevice.Android;               
                client.DownloadString(updateLink);
            }
            await con.SaveData("FirebaseToken", refreshedToken);
        }
	}
}
