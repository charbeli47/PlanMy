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
                UserCookie cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCookie>(usr);
                WebClient client = new WebClient();
                string updateLink = "https://planmy.me/maizonpub-api/add_device.php?action=updatedevice&userid=" + cookie.user.id + "&token=" + refreshedToken;                
                client.DownloadString(updateLink);
            }
            await con.SaveData("FirebaseToken", refreshedToken);
        }
	}
}
