using System.Net.Http.Headers;
using System.Web;

namespace NASA_APOD
{
    /// <summary>
    /// Disposable APOD Client that gives access to querying the APOD API
    /// </summary>
    public sealed class APOD_Client : IDisposable
    {
        private bool _disposed;
        private HttpClient _httpClient;
        private string _apiKey;

        /// <summary>
        /// Class containing information on returned APOD results
        /// </summary>
        public class APOD_File
        {
            public string? copyright;
            public string? date;
            public string? explanation;
            public string? hdurl;
            public string? media_type;
            public string? service_version;
            public string? title;
            public string? url;
        }

        /// <summary>
        /// Constructor for APOD Client
        /// </summary>
        /// <param name="apiKey">Your API key as found on https://api.nasa.gov/#signUp</param>
        public APOD_Client(string apiKey)
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _apiKey = apiKey;
        }

        /// <summary>
        /// Disposes the APOD Client
        /// </summary>
        public void Dispose()
        {
            if (_disposed) return;
            _httpClient.Dispose();
            _disposed = true;
        }

        /// <summary>
        /// Queries the APOD API with specified parameters (some may not be used together, please check https://api.nasa.gov/ under APOD)
        /// </summary>
        /// <param name="date">The date of the APOD you would like (cannot be used with start_date nor end_date)</param>
        /// <param name="start_date">The start date of all of the APOD's you would like (must be used with end_date)</param>
        /// <param name="end_date">The end date of all the APOD's you would like (must be used with start_date)</param>
        /// <param name="count">The amount of random APOD's you would like returned (cannot be used with date, start_date, nor end_date)</param>
        /// <param name="thumbs">If true, returns the URL of a video thumbnail when applicable</param>
        /// <returns>A list of all returned API Files</returns>
        /// <exception cref="ObjectDisposedException">Can occur as a result of accessing a disposed object</exception>
        /// <exception cref="Exception">Can occur as a result of a variety of errors</exception>
        public async Task<APOD_File[]> QueryAsync(DateTime? date = null, DateTime? start_date = null, DateTime? end_date = null, int? count = null, bool thumbs = false)
        {
            if (_disposed) throw new ObjectDisposedException(nameof(APOD_Client));

            if (start_date != null && date != null) throw new Exception("start_date cannot be used with date");
            if (end_date != null && start_date == null) throw new Exception("end_date must be used with start_date");
            if (count != null && (start_date != null || end_date != null)) throw new Exception("count cannot be used with start_date or end_date");

            using (HttpRequestMessage request = new HttpRequestMessage())
            {
                string requestUrl = @"https://api.nasa.gov/planetary/apod";
                var query = HttpUtility.ParseQueryString(string.Empty);
                if (date != null) query["date"] = ((DateTime)date).ToString("yyyy-MM-dd");
                if (start_date != null) query["start_date"] = ((DateTime)start_date).ToString("yyyy-MM-dd");
                if (end_date != null) query["end_date"] = ((DateTime)end_date).ToString("yyyy-MM-dd");
                if (count != null) query["count"] = count.ToString();
                if (thumbs != false) query["thumbs"] = thumbs.ToString();
                query["api_key"] = _apiKey;
                requestUrl += $"?{query}";
                request.RequestUri = new Uri(requestUrl);

                using (HttpResponseMessage response = await _httpClient.SendAsync(request))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        List<APOD_File> returnFiles = new List<APOD_File>();
                        if (start_date != null || end_date != null || count != null)
                        {
                            APOD_File[] files = await response.Content.ReadAsAsync<APOD_File[]>();
                            foreach (var file in files)
                            {
                                returnFiles.Add(file);
                            }
                        }
                        else
                        {
                            APOD_File file = await response.Content.ReadAsAsync<APOD_File>();
                            returnFiles.Add(file);
                        }
                        return returnFiles.ToArray();
                    }
                    else
                    {
                        throw new Exception($"ERROR {response.StatusCode}: {response.ReasonPhrase}");
                    }
                }
            }
        }
    }
}