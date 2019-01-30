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
            IFolder folder = await rootFolder.CreateFolderAsync("PlanMySubFolder", CreationCollisionOption.OpenIfExists);

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
            IFolder folder = await rootFolder.CreateFolderAsync("PlanMySubFolder", CreationCollisionOption.OpenIfExists);

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
                var folder = await rootFolder.GetFolderAsync("PlanMySubFolder");
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
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                StringContent content = new StringContent(json);
                //HTTP DELETE
                var postTask = client.PostAsync(url, content);
                postTask.Wait();

                var result = postTask.Result;
                return result.Content.ToString();
            }
        }
        public string PutToServer(string url, string json)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                StringContent content = new StringContent(json);
                //HTTP DELETE
                var postTask = client.PutAsync(url, content);
                postTask.Wait();

                var result = postTask.Result;
                return result.Content.ToString();
            }
        }
        public string DeleteFromServer(string url)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);

                //HTTP DELETE
                var deleteTask = client.DeleteAsync(url);
                deleteTask.Wait();

                var result = deleteTask.Result;
                return result.Content.ToString();
            }
        }
        public async void DeleteData(string key)
        {
            try
            {
                IFolder rootFolder = FileSystem.Current.LocalStorage;
                IFolder folder = await rootFolder.CreateFolderAsync("PlanMySubFolder", CreationCollisionOption.OpenIfExists);
                IFile file = await folder.GetFileAsync(key + ".txt");
                await file.DeleteAsync();
                //await file.WriteAllTextAsync("");
            }
            catch { }
        }
		
	}
}
