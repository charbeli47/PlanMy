using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PlanMy.Library
{
    public class Connect
    {
        public async Task<string> DownloadData(string url, string postData)
        {
            //string lang = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            if (postData != "")
                url += "?" + postData;
            HttpClient client = new HttpClient();
			//StringContent queryString = new StringContent(postData);
			try
			{
				string response = await client.GetStringAsync(new System.Uri(url));

				//response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
				//response.EnsureSuccessStatusCode();
				string responseBody = response;

				return responseBody;
			}
			catch(Exception ex)
			{
				return " ";
			}
			
        }
        public async Task SaveData(string key, string value)
        {
            // get hold of the file system
            IFolder rootFolder = FileSystem.Current.LocalStorage;

            // create a folder, if one does not exist already
            IFolder folder = await rootFolder.CreateFolderAsync("MySubFolder", CreationCollisionOption.OpenIfExists);

            // create a file, overwriting any existing file
            IFile file = await folder.CreateFileAsync(key + ".txt", CreationCollisionOption.ReplaceExisting);

            // populate the file with some text
            await file.WriteAllTextAsync(value);
        }
        public async void SaveDataSync(string key, string value)
        {
            // get hold of the file system
            IFolder rootFolder = FileSystem.Current.LocalStorage;

            // create a folder, if one does not exist already
            IFolder folder = await rootFolder.CreateFolderAsync("MySubFolder", CreationCollisionOption.OpenIfExists);

            // create a file, overwriting any existing file
            IFile file = await folder.CreateFileAsync(key + ".txt", CreationCollisionOption.ReplaceExisting);

            // populate the file with some text
            await file.WriteAllTextAsync(value);
        }
        public async Task<string> GetData(string key)
        {
            try
            {
                IFolder rootFolder = FileSystem.Current.LocalStorage;
                var folder = await rootFolder.GetFolderAsync("MySubFolder");
                IFile file = await folder.GetFileAsync(key + ".txt");
                return await file.ReadAllTextAsync();
            }
            catch
            {
                return "";
            }
        }
        public string PostToServer(string url, string json)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            string result = "";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {


                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            return result;
        }
        public async void DeleteData(string key)
        {
            try
            {
                IFolder rootFolder = FileSystem.Current.LocalStorage;
                IFolder folder = await rootFolder.CreateFolderAsync("MySubFolder", CreationCollisionOption.OpenIfExists);
                IFile file = await folder.CreateFileAsync(key + ".txt", CreationCollisionOption.ReplaceExisting);
                await file.WriteAllTextAsync("");
            }
            catch { }
        }
		
	}
}
