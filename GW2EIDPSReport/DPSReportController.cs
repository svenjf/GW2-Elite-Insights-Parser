﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GW2EIDPSReport.DPSReportJsons;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace GW2EIDPSReport
{
    public static class DPSReportController
    {

        private static bool IsUserTokenValid(string userToken)
        {
            return userToken != null && userToken.Length > 0;
        }

        private static readonly DefaultContractResolver DefaultJsonContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };

        private class DPSReportUserTokenResponse
        {
            public string UserToken { get; set; }
        }

        private static string BaseUploadContentURL { get; } = "https://dps.report/uploadContent?json=1";
        private static string BaseGetUploadsURL { get; } = "https://dps.report/getUploads?page=";
        private static string BaseGetUserTokenURL { get; } = "https://dps.report/getUserToken";
        private static string BaseGetUploadMetadataURL { get; } = "https://dps.report/getUploadMetadata?";
        private static string BaseGetJsonURL { get; } = "https://dps.report/getJson?";

        private static string GetURL(string baseURL, string userToken)
        {
            string url = baseURL;
            if (IsUserTokenValid(userToken))
            {
                url += "&userToken=" + userToken;
            }
            return url;
        }

        private static string GetUploadURL(string baseURL, string userToken, bool anonymous = false, bool detailedWvW = false)
        {
            string url = GetURL(baseURL, userToken);
            if (anonymous)
            {
                url += "&anonymous=true";
            }
            if (detailedWvW)
            {
                url += "&detailedwvw=true";
            }
            return url;
        }

        public static DPSReportUploadObject UploadUsingEI(FileInfo fi, List<string> traces, string userToken, bool anonymous = false, bool detailedWvW = false)
        {
            return UploadToDPSR(fi, GetUploadURL(BaseUploadContentURL, userToken, anonymous, detailedWvW) + "&generator=ei", traces);
        }

        public static DPSReportGetUploadsObject GetUploads(List<string> traces, string userToken, int page = 1)
        {
            return GetDPSReportResponse<DPSReportGetUploadsObject>("GetUploads", GetURL(BaseGetUploadsURL + page, userToken), traces);
        }
        public static string GenerateUserToken(List<string> traces)
        {
            DPSReportUserTokenResponse responseItem = GetDPSReportResponse<DPSReportUserTokenResponse>("GenerateUserToken", BaseGetUserTokenURL, traces);
            if (responseItem != null)
            {
                return responseItem.UserToken;
            }
            return "";
        }
        public static DPSReportUploadObject GetUploadMetaDataWithID(string id, List<string> traces)
        {
            if (id == null || id.Length == 0)
            {
                throw new InvalidDataException("Missing ID for GetUploadMetaData end point");
            }
            return GetDPSReportResponse<DPSReportUploadObject>("GetUploadMetaDataWithID", BaseGetUploadMetadataURL + "id=" + id, traces);
        }
        public static DPSReportUploadObject GetUploadMetaDataWithPermalink(string permalink, List<string> traces)
        {
            if (permalink == null || permalink.Length == 0)
            {
                throw new InvalidDataException("Missing Permalink for GetUploadMetaData end point");
            }
            return GetDPSReportResponse<DPSReportUploadObject>("GetUploadMetaDataWithPermalink", BaseGetUploadMetadataURL + "permalink=" + permalink, traces);
        }

        public static T GetJsonWithID<T>(string id, List<string> traces)
        {
            if (id == null || id.Length == 0)
            {
                throw new InvalidDataException("Missing ID for GetJson end point");
            }
            return GetDPSReportResponse<T>("GetJsonWithID", BaseGetJsonURL + "id=" + id, traces);
        }
        public static T GetJsonWithPermalink<T>(string permalink, List<string> traces)
        {
            if (permalink == null || permalink.Length == 0)
            {
                throw new InvalidDataException("Missing Permalink for GetJson end point");
            }
            return GetDPSReportResponse<T>("GetJsonWithPermalink", BaseGetJsonURL + "permalink=" + permalink, traces);
        }

        /*private static string UploadRaidar(FileInfo fi)
        {
            //string fileName = fi.Name;
            //byte[] fileContents = File.ReadAllBytes(fi.FullName);
            //Uri webService = new Uri(@"https://www.gw2raidar.com/api/v2/encounters/new");
            //HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, webService);
            //requestMessage.Headers.ExpectContinue = false;

            //MultipartFormDataContent multiPartContent = new MultipartFormDataContent("----MyGreatBoundary");
            //ByteArrayContent byteArrayContent = new ByteArrayContent(fileContents);
            //byteArrayContent.Headers.Add("Content-Type", "application/octet-stream");
            //multiPartContent.Add(byteArrayContent, "file", fileName);
            //requestMessage.Content = multiPartContent;

            //HttpClient httpClient = new HttpClient();
            //try
            //{
            //    Task<HttpResponseMessage> httpRequest = httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseContentRead, CancellationToken.None);
            //    HttpResponseMessage httpResponse = httpRequest.Result;
            //    HttpStatusCode statusCode = httpResponse.StatusCode;
            //    HttpContent responseContent = httpResponse.Content;

            //    if (responseContent != null)
            //    {
            //        Task<String> stringContentsTask = responseContent.ReadAsStringAsync();
            //        String stringContents = stringContentsTask.Result;
            //        int first = stringContents.IndexOf('{');
            //        int length = stringContents.LastIndexOf('}') - first + 1;
            //        String JSONFormat = stringContents.Substring(first, length);
            //        DPSReportsResponseItem item = JsonConvert.DeserializeObject<DPSReportsResponseItem>(JSONFormat);
            //        String logLink = item.permalink;
            //        return logLink;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return ex.Message;
            //    // Console.WriteLine(ex.Message);
            //}
            return "";
        }*/
        private static T GetDPSReportResponse<T>(string requestName, string URI, List<string> traces)
        {
            const int tentatives = 5;
            for (int i = 0; i < tentatives; i++)
            {
                traces.Add(requestName + " tentative");
                var webService = new Uri(@URI);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, webService);
                requestMessage.Headers.ExpectContinue = false;

                var httpClient = new HttpClient();
                try
                {
                    Task<HttpResponseMessage> httpRequest = httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseContentRead, CancellationToken.None);
                    HttpResponseMessage httpResponse = httpRequest.Result;
                    HttpStatusCode statusCode = httpResponse.StatusCode;
                    HttpContent responseContent = httpResponse.Content;

                    if (statusCode != HttpStatusCode.OK)
                    {
                        throw new HttpRequestException(statusCode.ToString());
                    }

                    if (responseContent != null)
                    {
                        Task<string> stringContentsTask = responseContent.ReadAsStringAsync();
                        string stringContents = stringContentsTask.Result;
                        T item = JsonConvert.DeserializeObject<T>(stringContents, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            ContractResolver = DefaultJsonContractResolver,
                            StringEscapeHandling = StringEscapeHandling.EscapeHtml
                        });
                        traces.Add(requestName + " tentative successful");
                        return item;
                    }
                }
                catch (Exception e)
                {
                    traces.Add(requestName + " tentative failed: " + e.Message);
                }
                finally
                {
                    httpClient.Dispose();
                    requestMessage.Dispose();
                }
            }
            return default;
        }
        private static DPSReportUploadObject UploadToDPSR(FileInfo fi, string URI, List<string> traces)
        {
            string fileName = fi.Name;
            byte[] fileContents = File.ReadAllBytes(fi.FullName);
            const int tentatives = 5;
            for (int i = 0; i < tentatives; i++)
            {
                traces.Add("Upload tentative");
                var webService = new Uri(@URI);
                var requestMessage = new HttpRequestMessage(HttpMethod.Post, webService);
                requestMessage.Headers.ExpectContinue = false;

                var multiPartContent = new MultipartFormDataContent("----MyGreatBoundary");
                var byteArrayContent = new ByteArrayContent(fileContents);
                byteArrayContent.Headers.Add("Content-Type", "application/octet-stream");
                multiPartContent.Add(byteArrayContent, "file", fileName);
                //multiPartContent.Add(new StringContent("generator=ei"), "gen", "ei");
                requestMessage.Content = multiPartContent;

                var httpClient = new HttpClient();
                try
                {
                    Task<HttpResponseMessage> httpRequest = httpClient.SendAsync(requestMessage, HttpCompletionOption.ResponseContentRead, CancellationToken.None);
                    HttpResponseMessage httpResponse = httpRequest.Result;
                    HttpStatusCode statusCode = httpResponse.StatusCode;
                    HttpContent responseContent = httpResponse.Content;

                    if (statusCode != HttpStatusCode.OK)
                    {
                        throw new HttpRequestException(statusCode.ToString());
                    }

                    if (responseContent != null)
                    {
                        Task<string> stringContentsTask = responseContent.ReadAsStringAsync();
                        string stringContents = stringContentsTask.Result;
                        DPSReportUploadObject item = JsonConvert.DeserializeObject<DPSReportUploadObject>(stringContents, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            ContractResolver = DefaultJsonContractResolver,
                            StringEscapeHandling = StringEscapeHandling.EscapeHtml
                        });
                        if (item.Error != null)
                        {
                            throw new InvalidOperationException(item.Error);
                        }
                        traces.Add("Upload tentative successful");
                        // Added file output for uploaded objects
                        string nameOfFile = "dpsReport-links.txt";
                        string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        using (StreamWriter outputFile = File.AppendText(Path.Combine(docPath, nameOfFile)))
                        {
                            outputFile.WriteLine($"{item.Permalink}");
                        }
                        // End addition
                        return item;
                    }
                }
                catch (Exception e)
                {
                    traces.Add("Upload tentative failed: " + e.Message);
                }
                finally
                {
                    byteArrayContent.Dispose();
                    httpClient.Dispose();
                    requestMessage.Dispose();
                }
            }
            return null;
        }

    }

}
