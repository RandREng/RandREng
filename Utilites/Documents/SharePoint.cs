using Microsoft.SharePoint.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security;
using System.Text;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using SP = Microsoft.SharePoint.Client;

namespace RandREng.Utility.Documents
{
	//public class SharePoint
	//{
	//	private static void exportBatchSharepoint(string website, string username, string password, string path, List<Dir> subdirs)
	//	{
	//		try
	//		{
	//			if (string.IsNullOrEmpty(website))
	//				throw new Exception("Error: Sharepoint Not configured correctly.");

	//			using (Microsoft.SharePoint.Client.ClientContext client = new ClientContext(website))
	//			{
	//				System.Diagnostics.Debug.WriteLine("Connecting to Sharepoint site...");

	//				client.Credentials = new System.Net.NetworkCredential(username, password);
	//				System.Diagnostics.Debug.WriteLine("Connected.");

	//				var root_folder = client.Web.GetFolderByServerRelativeUrl(path);
	//				if (root_folder == null)
	//					root_folder = client.Web.Folders.Add(path);

	//				System.Diagnostics.Debug.WriteLine("\tCreating Sharepoint Sub-Directory \"" + path + "\".");

	//				foreach (var dir in subdirs)
	//				{
	//					string dir_path = Uri.EscapeUriString(dir.name).Replace("?", "_");
	//					System.Diagnostics.Debug.WriteLine("\t\tCreating Document Directory \"" + dir_path + "\".");

	//					var dir_folder = root_folder.Folders.Add(dir_path);

	//					foreach (var doc in dir.documents)
	//					{
	//						string doc_path = Path.GetFileName(doc);

	//						var uplfileStream = System.IO.File.ReadAllBytes(doc);
	//						dir_folder.Files.Add(new FileCreationInformation()
	//						{
	//							Content = uplfileStream,
	//							Overwrite = true,
	//							Url = doc_path
	//						});
	//					}

	//				}

	//				System.Diagnostics.Debug.WriteLine("\tUploading to Sharepoint Server is Done.");
	//			}
	//		}
	//		catch (Exception ex)
	//		{
	//			System.Diagnostics.Debug.WriteLine("ERROR: " + ex.Message);
	//		}
	//	}

	//	private static void UploadFileToSharePoint(string SiteUrl, string DocLibrary, string ClientSubFolder, string FileName, string Login, string Password)
	//	{
	//		try
	//		{
	//			#region ConnectToSharePoint
	//			var securePassword = new SecureString();
	//			foreach (char c in Password)
	//			{ securePassword.AppendChar(c); }
	//			var onlineCredentials = new SharePointOnlineCredentials(Login, securePassword);
	//			#endregion
	//			#region Insert the data
	//			using (ClientContext CContext = new ClientContext(SiteUrl))
	//			{
	//				CContext.Credentials = onlineCredentials;
	//				Web web = CContext.Web;
	//				FileCreationInformation newFile = new FileCreationInformation();
	//				byte[] FileContent = System.IO.File.ReadAllBytes(FileName);
	//				newFile.ContentStream = new MemoryStream(FileContent);
	//				newFile.Url = Path.GetFileName(FileName);
	//				List DocumentLibrary = web.Lists.GetByTitle(DocLibrary);
	//				Folder Clientfolder = null;
	//				if (ClientSubFolder == "")
	//				{
	//					Clientfolder = DocumentLibrary.RootFolder;
	//				}
	//				else
	//				{
	//					Clientfolder = DocumentLibrary.RootFolder.Folders.Add(ClientSubFolder);
	//					Clientfolder.Update();
	//				}
	//				Microsoft.SharePoint.Client.File uploadFile = Clientfolder.Files.Add(newFile);
	//				CContext.Load(DocumentLibrary);
	//				CContext.Load(uploadFile);
	//				CContext.ExecuteQuery();
	//				Console.ForegroundColor = ConsoleColor.Green;
	//				Console.WriteLine("The File has been uploaded" + Environment.NewLine + "FileUrl -->" + SiteUrl + "/" + DocLibrary + "/" + ClientSubFolder + "/" + Path.GetFileName(FileName));
	//			}
	//			#endregion
	//		}
	//		catch (Exception exp)
	//		{
	//			Console.ForegroundColor = ConsoleColor.Red;
	//			Console.WriteLine(exp.Message + Environment.NewLine + exp.StackTrace);
	//		}
	//		finally
	//		{
	//			Console.ReadLine();
	//		}
	//	}

	//	public void UploadDocument(string siteURL, string serverRelativeDestinationFilenamePath, string fullLocalFilenamePathToImport)
	//	{
	//		using (ClientContext context = new ClientContext(siteURL))
	//		{
	//			// Credentials hardcoded here for brevity, in reality you
	//			// want to store them in a secure location or request user to input them
	//			string password = "Ox0ypVcVgXXXX";
	//			string account = "admin@xxx.com";
	//			var secret = new SecureString();
	//			foreach (char c in password)
	//			{
	//				secret.AppendChar(c);
	//			}
	//			context.Credentials = new SP.SharePointOnlineCredentials(account, secret);

	//			using (FileStream fs = new FileStream(fullLocalFilenamePathToImport, FileMode.OpenOrCreate))
	//			{
	//				Microsoft.SharePoint.Client.File.SaveBinaryDirect(context, serverRelativeDestinationFilenamePath, fs, true);
	//			}
	//			var newFile = context.Web.GetFileByServerRelativeUrl(serverRelativeDestinationFilenamePath);

	//			context.Load(newFile);
	//			context.ExecuteQuery();

	//			//check out to make sure not to create multiple versions
	//			newFile.CheckOut();

	//			var item = newFile.ListItemAllFields;

	//			// update some metadata if you need to, for example:
	//			// item["Title"] = "My Uploaded File";
	//			// item["CustomFieldName"] = "Some Custom Value";
	//			// item.Update();
	//			// context.ExecuteQuery();      

	//			// use OverwriteCheckIn type to make sure not to create multiple versions 
	//			newFile.CheckIn(string.Empty, CheckinType.OverwriteCheckIn);
	//		}
	//	}

	//}

    //public class AuthenticationManager : IDisposable
    //{
    //    private static readonly HttpClient httpClient = new HttpClient();
    //    private const string tokenEndpoint = "https://login.microsoftonline.com/common/oauth2/token";

    //    private const string defaultAADAppId = "3de78f25-cbf5-4ec4-b9af-349c91904dc5";

    //    // Token cache handling  
    //    private static readonly SemaphoreSlim semaphoreSlimTokens = new SemaphoreSlim(1);
    //    private AutoResetEvent tokenResetEvent = null;
    //    private readonly ConcurrentDictionary<string, string> tokenCache = new ConcurrentDictionary<string, string>();
    //    private bool disposedValue;

    //    internal class TokenWaitInfo
    //    {
    //        public RegisteredWaitHandle Handle = null;
    //    }

    //    public ClientContext GetContext(Uri web, string userPrincipalName, SecureString userPassword)
    //    {
    //        var context = new ClientContext(web);

    //        context.ExecutingWebRequest += (sender, e) =>
    //        {
    //            string accessToken = EnsureAccessTokenAsync(new Uri($"{web.Scheme}://{web.DnsSafeHost}"), userPrincipalName, new System.Net.NetworkCredential(string.Empty, userPassword).Password).GetAwaiter().GetResult();
    //            e.WebRequestExecutor.RequestHeaders["Authorization"] = "Bearer " + accessToken;
    //        };

    //        return context;
    //    }


    //    public async Task<string> EnsureAccessTokenAsync(Uri resourceUri, string userPrincipalName, string userPassword)
    //    {
    //        string accessTokenFromCache = TokenFromCache(resourceUri, tokenCache);
    //        if (accessTokenFromCache == null)
    //        {
    //            await semaphoreSlimTokens.WaitAsync().ConfigureAwait(false);
    //            try
    //            {
    //                // No async methods are allowed in a lock section  
    //                string accessToken = await AcquireTokenAsync(resourceUri, userPrincipalName, userPassword).ConfigureAwait(false);
    //                Console.WriteLine($"Successfully requested new access token resource {resourceUri.DnsSafeHost} for user {userPrincipalName}");
    //                AddTokenToCache(resourceUri, tokenCache, accessToken);

    //                // Register a thread to invalidate the access token once's it's expired  
    //                tokenResetEvent = new AutoResetEvent(false);
    //                TokenWaitInfo wi = new TokenWaitInfo();
    //                wi.Handle = ThreadPool.RegisterWaitForSingleObject(
    //                    tokenResetEvent,
    //                    async (state, timedOut) =>
    //                    {
    //                        if (!timedOut)
    //                        {
    //                            TokenWaitInfo wi1 = (TokenWaitInfo)state;
    //                            if (wi1.Handle != null)
    //                            {
    //                                wi1.Handle.Unregister(null);
    //                            }
    //                        }
    //                        else
    //                        {
    //                            try
    //                            {
    //                                // Take a lock to ensure no other threads are updating the SharePoint Access token at this time  
    //                                await semaphoreSlimTokens.WaitAsync().ConfigureAwait(false);
    //                                RemoveTokenFromCache(resourceUri, tokenCache);
    //                                Console.WriteLine($"Cached token for resource {resourceUri.DnsSafeHost} and user {userPrincipalName} expired");
    //                            }
    //                            catch (Exception ex)
    //                            {
    //                                Console.WriteLine($"Something went wrong during cache token invalidation: {ex.Message}");
    //                                RemoveTokenFromCache(resourceUri, tokenCache);
    //                            }
    //                            finally
    //                            {
    //                                semaphoreSlimTokens.Release();
    //                            }
    //                        }
    //                    },
    //                    wi,
    //                    (uint)CalculateThreadSleep(accessToken).TotalMilliseconds,
    //                    true
    //                );

    //                return accessToken;

    //            }
    //            finally
    //            {
    //                semaphoreSlimTokens.Release();
    //            }
    //        }
    //        else
    //        {
    //            Console.WriteLine($"Returning token from cache for resource {resourceUri.DnsSafeHost} and user {userPrincipalName}");
    //            return accessTokenFromCache;
    //        }
    //    }

    //    private async Task<string> AcquireTokenAsync(Uri resourceUri, string username, string password)
    //    {
    //        string resource = $"{resourceUri.Scheme}://{resourceUri.DnsSafeHost}";

    //        var clientId = defaultAADAppId;
    //        var body = $"resource={resource}&client_id={clientId}&grant_type=password&username={HttpUtility.UrlEncode(username)}&password={HttpUtility.UrlEncode(password)}";
    //        using (var stringContent = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded"))
    //        {

    //            var result = await httpClient.PostAsync(tokenEndpoint, stringContent).ContinueWith((response) =>
    //            {
    //                return response.Result.Content.ReadAsStringAsync().Result;
    //            }).ConfigureAwait(false);

    //            var tokenResult = JsonSerializer.Deserialize<JsonElement>(result);
    //            var token = tokenResult.GetProperty("access_token").GetString();
    //            return token;
    //        }
    //    }

    //    private static string TokenFromCache(Uri web, ConcurrentDictionary<string, string> tokenCache)
    //    {
    //        if (tokenCache.TryGetValue(web.DnsSafeHost, out string accessToken))
    //        {
    //            return accessToken;
    //        }

    //        return null;
    //    }

    //    private static void AddTokenToCache(Uri web, ConcurrentDictionary<string, string> tokenCache, string newAccessToken)
    //    {
    //        if (tokenCache.TryGetValue(web.DnsSafeHost, out string currentAccessToken))
    //        {
    //            tokenCache.TryUpdate(web.DnsSafeHost, newAccessToken, currentAccessToken);
    //        }
    //        else
    //        {
    //            tokenCache.TryAdd(web.DnsSafeHost, newAccessToken);
    //        }
    //    }

    //    private static void RemoveTokenFromCache(Uri web, ConcurrentDictionary<string, string> tokenCache)
    //    {
    //        tokenCache.TryRemove(web.DnsSafeHost, out string currentAccessToken);
    //    }

    //    private static TimeSpan CalculateThreadSleep(string accessToken)
    //    {
    //        var token = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(accessToken);
    //        var lease = GetAccessTokenLease(token.ValidTo);
    //        lease = TimeSpan.FromSeconds(lease.TotalSeconds - TimeSpan.FromMinutes(5).TotalSeconds > 0 ? lease.TotalSeconds - TimeSpan.FromMinutes(5).TotalSeconds : lease.TotalSeconds);
    //        return lease;
    //    }

    //    private static TimeSpan GetAccessTokenLease(DateTime expiresOn)
    //    {
    //        DateTime now = DateTime.UtcNow;
    //        DateTime expires = expiresOn.Kind == DateTimeKind.Utc ? expiresOn : TimeZoneInfo.ConvertTimeToUtc(expiresOn);
    //        TimeSpan lease = expires - now;
    //        return lease;
    //    }

    //    protected virtual void Dispose(bool disposing)
    //    {
    //        if (!disposedValue)
    //        {
    //            if (disposing)
    //            {
    //                if (tokenResetEvent != null)
    //                {
    //                    tokenResetEvent.Set();
    //                    tokenResetEvent.Dispose();
    //                }
    //            }

    //            disposedValue = true;
    //        }
    //    }

    //    public void Dispose()
    //    {
    //        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method  
    //        Dispose(disposing: true);
    //        GC.SuppressFinalize(this);
    //    }
    //}
}   

