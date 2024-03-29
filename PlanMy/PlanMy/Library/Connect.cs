﻿using PCLStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
			StringContent queryString = new StringContent(postData);
			try
			{
                var response = await client.GetAsync(url);                
                //response.EnsureSuccessStatusCode();
                string responseBody = "";
                if (response.IsSuccessStatusCode)
                {
                    responseBody = await response.Content.ReadAsStringAsync();
                }

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
        public async Task<string> PostToServer(string url, MultipartFormDataContent data)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                //HTTP DELETE
                var postTask = await client.PostAsync(url, data);

                var result = await postTask.Content.ReadAsStringAsync();
                return result;
            }
        }
        public async Task<string> PostToServer(string url, string json)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                //HTTP DELETE
                var postTask = await client.PostAsync(url, content);

                var result = await postTask.Content.ReadAsStringAsync();
                return result;
            }
        }
        public async Task<string> PutToServer(string url, string json)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                StringContent content = new StringContent(json);
                //HTTP DELETE
                var postTask = await client.PutAsync(url, content);

                var result = await postTask.Content.ReadAsStringAsync();
                return result;
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
