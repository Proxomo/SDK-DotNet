using System;
using System.Web;
using System.Collections.Generic;

namespace Proxomo
{
    public partial class ProxomoApi
    {
        internal static string _applicationID = string.Empty;
        internal static string _proxomoAPIKey = string.Empty;
        internal static string baseURL = string.Empty;
        private string contentType = "application/json";

        public string ApplicationID
        {
            get
            {
                return _applicationID;
            }
        }

        public string ProxomoAPIKey
        {
            get
            {
                return _proxomoAPIKey;
            }
        }

        private string _APIVersion = "v09";
        public string APIVersion
        {
            get
            {
                return _APIVersion;
            }
            set
            {
                _APIVersion = value;
            }
        }
        private Token _AuthToken = new Token();
        public Token AuthToken
        {
            get
            {
                return _AuthToken;
            }
            set
            {
                _AuthToken = value;
            }
        }
        private bool _ValidateSSLCert = true;
        public bool ValidateSSLCert
        {
            get
            {
                return _ValidateSSLCert;
            }
            set
            {
                _ValidateSSLCert = value;
            }
        }
        private CommunicationType _Format = CommunicationType.JSON;
        public CommunicationType Format
        {
            get
            {
                return _Format;
            }
            set
            {
                _Format = value;
            }
        }

        public ProxomoApi(string applicationID, string proxomoAPIKey, CommunicationType format = CommunicationType.JSON, bool validatessl = true, string url = "", Token token = null)
        {
            Init(applicationID, proxomoAPIKey, "v09", format, validatessl, url, token);
        }

        public ProxomoApi(string applicationID, string proxomoAPIKey, string version, CommunicationType format = CommunicationType.JSON, bool validatessl = true, string url = "", Token token = null)
        {
            Init(applicationID, proxomoAPIKey, version, format, validatessl, url, token);
        }

        private void Init(string applicationID, string proxomoAPIKey, string version, CommunicationType format, bool validatessl, string url, Token token = null)
        {
            APIVersion = version;
            _applicationID = applicationID;
            _proxomoAPIKey = proxomoAPIKey;
            this.ValidateSSLCert = validatessl;
            this.Format = format;

            if (format == CommunicationType.XML)
            {
                contentType = "text/xml";

                if (String.IsNullOrWhiteSpace(url))
                {
                    baseURL = String.Format("https://service.proxomo.com/{0}/xml", APIVersion);
                }
                else
                {
                    baseURL = String.Format("{0}/{1}/xml", url, APIVersion);
                }
            }
            else if (format == CommunicationType.JSON)
            {
                contentType = "application/json";

                if (String.IsNullOrWhiteSpace(url))
                {
                    baseURL = string.Format("https://service.proxomo.com/{0}/json", APIVersion);
                }
                else
                {
                    baseURL = String.Format("{0}/{1}/json", url, APIVersion);
                }
            }

            if (token != null)
            {
                if (token.ExpiresDate <= DateTime.Now)
                {
                    GetAuthToken();
                }
                else
                {
                    AuthToken = token;
                }
            }
            else
            {
                GetAuthToken();
            }
        }

        private void GetAuthToken()
        {
            string url = string.Format("{0}/security/accesstoken/get?applicationid={1}&proxomoAPIKey={2}", baseURL, HttpUtility.UrlEncode(_applicationID), HttpUtility.UrlEncode(_proxomoAPIKey));
           
            using (ProxomoWebRequest<Token> p = new ProxomoWebRequest<Token>(ValidateSSLCert, Format))
            {
                AuthToken = p.GetData(url, "GET", string.Empty);
            }
        }

        public void RefreshAuthToken()
        {
            GetAuthToken();
        }

        public bool IsAuthTokenExpired()
        {
            if (DateTime.Now < AuthToken.ExpiresDate)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #region    AppData Methods

        public string  AppDataAdd(AppData appData)
        {
            string url = string.Format("{0}/appdata", baseURL);
                        
            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "POST", contentType, Converter.Convert(appData, Format, false));
            }
        }
        public void AppDataDelete(string appDataID)
        {
            string url = string.Format("{0}/appdata/{1}", baseURL, appDataID);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                p.GetData(url, "DELETE", contentType);
            }
        }
        public AppData AppDataGet(string appDataID)
        {
            string url = string.Format("{0}/appdata/{1}", baseURL, appDataID);

            using (ProxomoWebRequest<AppData> p = new ProxomoWebRequest<AppData>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }
        public List<AppData> AppDataGetAll()
        {
            string url = string.Format("{0}/appdata", baseURL);

            using (ProxomoWebRequest<List<AppData>> p = new ProxomoWebRequest<List<AppData>>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }
        public void AppDataUpdate(AppData appData)
        {
            string url = string.Format("{0}/appdata", baseURL);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                p.GetData(url, "PUT", contentType, Converter.Convert(appData, Format, false));
            }
        }
        public List<AppData> AppDataSearch(string objectType)
        {
            string url = string.Format("{0}/appdata/search/objecttype/{1}", baseURL, objectType);

            using (ProxomoWebRequest<List<AppData>> p = new ProxomoWebRequest<List<AppData>>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }
        #endregion

        #region Counter Methods

        public Counter CounterGet(string id)
        {
            string url = string.Format("{0}/counters/{1}", baseURL, id);

            using (ProxomoWebRequest<Counter> p = new ProxomoWebRequest<Counter>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }
        public int CounterIncrement(string countername, int increment)
        {
            string url = string.Format("{0}/counters/{1}/{2}", baseURL, countername, increment);

            using (ProxomoWebRequest<int> p = new ProxomoWebRequest<int>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "POST", contentType);
            }
        }
        public List<Counter> CountersGet(string[] keys)
        {
            string url = string.Format("{0}/counters", baseURL);

            using (ProxomoWebRequest<List<Counter>> p = new ProxomoWebRequest<List<Counter>>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "POST", contentType, Converter.Convert<string[]>(keys, Format));
            }
        }
        #endregion

        #region CustomDataStorage

        public string CustomDataAdd<T>(T data, string partition = "")
        {
            string url = string.Format("{0}/customdata?partition={1}", baseURL, partition);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "POST", contentType, Converter.Convert(data, Format, false, true));
            }
        }

        public string CustomDataUpdate<T>(T data, string partition = "")
        {
            string url = string.Format("{0}/customdata?partition={1}", baseURL, partition);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "PUT", contentType, Converter.Convert(data, Format, false, true));
            }
        }

        public void CustomDataDelete(string tableName, string customDataID, string partition = "")
        {
            string url = string.Format("{0}/customdata/table/{1}/{2}?partition={3}", baseURL, tableName, customDataID, partition);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                p.GetData(url, "DELETE", contentType);
            }
        }

        public List<T> CustomDataSearch<T>(string tableName, string query, int maxResults, ref ContinuationTokens cTokens, string partition = "")
        {
            string url = string.Format("{0}/customdata/search/table/{1}?q={2}&maxresults={3}&partition={4}", baseURL, tableName, query, maxResults, partition);

            using (ProxomoWebRequest<List<T>> p = new ProxomoWebRequest<List<T>>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType, "", ref cTokens);
            }
        }

        public T CustomDataGet<T>(string tableName, string customDataID, string partition = "")
        {
            string url = string.Format("{0}/customdata/table/{1}/{2}?partition={3}", baseURL, tableName, customDataID, partition);

            using (ProxomoWebRequest<T> p = new ProxomoWebRequest<T>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }

        #endregion

        #region    Event Methods

        public string EventAdd(Event evt)
        {
            string url = string.Format("{0}/event", baseURL);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "POST", contentType, Converter.Convert(evt, Format, false));
            }
        }
        public Event EventGet(string eventID)
        {
            string url = string.Format("{0}/event/{1}", baseURL, eventID);

            using (ProxomoWebRequest<Event> p = new ProxomoWebRequest<Event>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }
        public void EventUpdate(Event evt)
        {
            string url = string.Format("{0}/event", baseURL);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                p.GetData(url, "PUT", contentType, Converter.Convert(evt, Format, false));
            }
        }
        public List<Event> EventsGetByDistance(decimal latitude, decimal longitude, decimal distance, DateTime starttime, DateTime endtime, string eventType = "")
        {
            string url = null;

            if (string.IsNullOrWhiteSpace(eventType))
            {
                url = string.Format("{0}/events/search/latitude/{1}/longitude/{2}/distance/{3}/start/{4}/end/{5}", baseURL, latitude, longitude, distance, Utility.ConvertToUnixTimestamp(starttime), Utility.ConvertToUnixTimestamp(endtime));
            }
            else
            {
                url = string.Format("{0}/events/search/latitude/{1}/longitude/{2}/distance/{3}/start/{4}/end/{5}?eventtype={6}", baseURL, latitude, longitude, distance, Utility.ConvertToUnixTimestamp(starttime), Utility.ConvertToUnixTimestamp(endtime), eventType);
            }

            using (ProxomoWebRequest<List<Event>> p = new ProxomoWebRequest<List<Event>>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }
        public List<Event> EventsGetByPerson(string personID, DateTime starttime, DateTime endtime, string eventType = "")
        {
            string url = null;

            if (string.IsNullOrWhiteSpace(eventType))
            {
                url = string.Format("{0}/events/search/personid/{1}/start/{2}/end/{3}", baseURL, personID, Utility.ConvertToUnixTimestamp(starttime), Utility.ConvertToUnixTimestamp(endtime));
            }
            else
            {
                url = string.Format("{0}/events/search/personid/{1}/start/{2}/end/{3}?eventtype={4}", baseURL, personID, Utility.ConvertToUnixTimestamp(starttime), Utility.ConvertToUnixTimestamp(endtime), eventType);
            }

            using (ProxomoWebRequest<List<Event>> p = new ProxomoWebRequest<List<Event>>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }

        #endregion

        #region    EventComment Methods

        public string EventCommentAdd(string eventID, EventComment comment)
        {
            string url = string.Format("{0}/event/{1}/comment", baseURL, eventID);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "POST", contentType, Converter.Convert(comment, Format, false));
            }
        }
        public List<EventComment> EventCommentsGet(string eventID)
        {
            string url = string.Format("{0}/event/{1}/comments", baseURL, eventID);

            using (ProxomoWebRequest<List<EventComment>> p = new ProxomoWebRequest<List<EventComment>>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }
        public void EventCommentUpdate(string eventID, EventComment comment)
        {
            string url = string.Format("{0}/event/{1}/comment", baseURL, eventID);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                p.GetData(url, "PUT", contentType, Converter.Convert(comment, Format, false));
            }
        }
        public void EventCommentDelete(string eventID, string commentID)
        {
            string url = string.Format("{0}/event/{1}comment/{2}", baseURL, eventID, commentID);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                p.GetData(url, "DELETE", contentType);
            }
        }

        #endregion

        #region    Event Participant Methods

        public List<EventParticipant> EventParticipantsGet(string eventID)
        {
            string url = string.Format("{0}/event/{1}/participants", baseURL, eventID);

            using (ProxomoWebRequest<List<EventParticipant>> p = new ProxomoWebRequest<List<EventParticipant>>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }
        public void EventParticipantInvite(string eventID, string personID)
        {
            string url = string.Format("{0}/event/{1}/participant/invite/personid/{2}", baseURL, eventID, personID);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                p.GetData(url, "PUT", contentType);
            }
        }

        public void EventParticipantsInvite(string eventID, string[] personIDs)
        {

            var personIDstoStrArray = string.Join(",", personIDs);

            string url = string.Format("{0}/event/{1}/participants/invite/personids/{2}", baseURL, eventID, personIDstoStrArray);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                p.GetData(url, "PUT", contentType);
            }

        }
        public void EventParticipantDelete(string eventID, string participantID)
        {
            string url = string.Format("{0}/event/{1}/participant/{2}", baseURL, eventID, participantID);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                p.GetData(url, "DELETE", contentType);
            }
        }

        public void EventRequestInvitation(string eventID, string personID)
        {
            string url = string.Format("{0}/event/{1}/requestinvite/personid/{2}", baseURL, eventID, personID);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                p.GetData(url, "PUT", contentType);
            }
        }
        public void EventRSVP(string eventID, EventParticipantStatus participantStatus, string personID)
        {
            string url = string.Format("{0}/event/{1}/rsvp/personid/{2}/participantstatus/{3}", baseURL, eventID, personID, Convert.ToInt16(participantStatus));

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                p.GetData(url, "PUT", contentType);
            }
        }

        #endregion

        #region Event AppData Methods

        public string EventAppDataAdd(string EventID, AppData aData)
        {
            string url = string.Format("{0}/event/{1}/appdata", baseURL, EventID);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "POST", contentType, Converter.Convert(aData, Format, false));
            }
        }
        public void EventAppDataDelete(string EventID, string appDataID)
        {
            string url = string.Format("{0}/event/{1}/appdata/{2}", baseURL, EventID, appDataID);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                p.GetData(url, "DELETE", contentType);
            }
        }
        public AppData EventAppDataGet(string EventID, string appDataID)
        {
            string url = string.Format("{0}/event/{1}/appdata/{2}", baseURL, EventID, appDataID);

            using (ProxomoWebRequest<AppData> p = new ProxomoWebRequest<AppData>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }
        public List<AppData> EventAppDataGetAll(string EventID)
        {
            string url = string.Format("{0}/event/{1}/appdata", baseURL, EventID);

            using (ProxomoWebRequest<List<AppData>> p = new ProxomoWebRequest<List<AppData>>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }
        public void EventAppDataUpdate(string EventID, AppData aData)
        {
            string url = string.Format("{0}/event/{1}/appdata", baseURL, EventID);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                p.GetData(url, "PUT", contentType, Converter.Convert(aData, Format, false));
            }
        }

        #endregion

        #region    Friends Methods

        public List<Friend> FriendsGet(string personID)
        {
            string url = string.Format("{0}/friends/personid/{1}", baseURL, personID);

            using (ProxomoWebRequest<List<Friend>> p = new ProxomoWebRequest<List<Friend>>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }
        public void FriendInvite(string friendA, string friendB)
        {
            string url = string.Format("{0}/friend/invite/frienda/{1}/friendb/{2}", baseURL, friendA, friendB);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                p.GetData(url, "PUT", contentType);
            }
        }
        public void FriendBySocialNetworkInvite(SocialNetwork socialnetwork, string frienda, string friendb)
        {
            string url = string.Format("{0}/friend/invite/frienda/{1}/friendb/{2}/socialnetwork/{3}", baseURL, frienda, friendb, Convert.ToInt16(socialnetwork));

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                p.GetData(url, "PUT", contentType);
            }
        }
        public void FriendshipRespond(FriendResponse response, string friendA, string friendB)
        {
            string url = string.Format("{0}/friend/respond/frienda/{1}/friendb/{2}/friendresponse/{3}", baseURL, friendA, friendB, Convert.ToInt16(response));

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                p.GetData(url, "PUT", contentType);
            }
        }        
        public List<SocialNetworkFriend> FriendsSocialNetworkGet(SocialNetwork socialNetwork, string personID)
        {

            string url = string.Format("{0}/friends/personid/{1}/socialnetwork/{2}", baseURL, personID, Convert.ToInt16(socialNetwork));

            using (ProxomoWebRequest<List<SocialNetworkFriend>> p = new ProxomoWebRequest<List<SocialNetworkFriend>>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }
        public List<SocialNetworkPFriend> FriendsSocialNetworkAppGet(SocialNetwork socialNetwork, string personID)
        {
            string url = string.Format("{0}/friends/app/personid/{1}/socialnetwork/{2}", baseURL, personID, Convert.ToInt16(socialNetwork));

            using (ProxomoWebRequest<List<SocialNetworkPFriend>> p = new ProxomoWebRequest<List<SocialNetworkPFriend>>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }

        #endregion

        #region GeoCode Methods

        public GeoCode GeoCodebyAddress(string address)
        {
            string url = string.Format("{0}/geo/lookup/address/{1}", baseURL, HttpUtility.UrlEncode(address));

            using (ProxomoWebRequest<GeoCode> p = new ProxomoWebRequest<GeoCode>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }

        }
        public Location ReverseGeocode(string latitude, string longitude)
        {
            string url = string.Format("{0}/geo/lookup/latitude/{1}/longitude/{2}", baseURL, latitude, longitude);

            using (ProxomoWebRequest<Location> p = new ProxomoWebRequest<Location>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }
        public GeoIP GeoCodeByIPAddress(string ipAddress)
        {
            string url = string.Format("{0}/geo/lookup/ip/{1}", baseURL, ipAddress);

            using (ProxomoWebRequest<GeoIP> p = new ProxomoWebRequest<GeoIP>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }
        
        #endregion

        #region    Location Methods

        public string LocationAdd(Location location)
        {
            string url = string.Format("{0}/location", baseURL);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "POST", contentType, Converter.Convert(location, Format, false));
            }
        }
        public Location LocationGet(string locationID)
        {
            string url = string.Format("{0}/location/{1}", baseURL, locationID);

            using (ProxomoWebRequest<Location> p = new ProxomoWebRequest<Location>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }

        }
        public void LocationUpdate(Location location)
        {
            string url = string.Format("{0}/location", baseURL);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                p.GetData(url, "PUT", contentType, Converter.Convert(location, Format, false));
            }
        }
        public void LocationDelete(string locationID)
        {
            string url = string.Format("{0}/location/{1}", baseURL, locationID);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                p.GetData(url, "DELETE", contentType);
            }
        }
        public List<Category> LocationCategoriesGet()
        {
            string url = string.Format("{0}/location/categories", baseURL);

            using (ProxomoWebRequest<List<Category>> p = new ProxomoWebRequest<List<Category>>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }
        public List<Location> LocationsSearchByAddress(string address, string q = "", string category = "", double radius = 2, LocationSearchScope scope = LocationSearchScope.ApplicationOnly, int maxresults = 10, string personid = "")
        {
            string url = string.Format("{0}/locations/search", baseURL) + Utility.FormatQueryString(address, string.Empty, string.Empty, q, category, radius, scope, maxresults, personid);

            using (ProxomoWebRequest<List<Location>> p = new ProxomoWebRequest<List<Location>>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }

        }
        public List<Location> LocationsSearchByLocationType(string locationtype)
        {
            string url = string.Format("{0}/locations/search/locationtype/{1}", baseURL, locationtype);

            using (ProxomoWebRequest<List<Location>> p = new ProxomoWebRequest<List<Location>>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }

        }
        public List<Location> LocationsSearchByGPS(string latitude, string longitude, string q = "", string category = "", double radius = 2, LocationSearchScope scope = LocationSearchScope.ApplicationOnly, int maxresults = 10, string personid = "")
        {
            string url = string.Format("{0}/locations/search/latitude/{1}/longitude/{2}", baseURL, latitude, longitude) + Utility.FormatQueryString(string.Empty, string.Empty, string.Empty, q, category, radius, scope, maxresults, personid);

            using (ProxomoWebRequest<List<Location>> p = new ProxomoWebRequest<List<Location>>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }

        }
        public List<Location> LocationsSearchByIPAddress(string ipAddress, string q = "", string category = "", double radius = 2, LocationSearchScope scope = LocationSearchScope.ApplicationOnly, int maxresults = 10, string personid = "")
        {
            string url = string.Format("{0}/locations/search/ip/{1}", baseURL, ipAddress) + Utility.FormatQueryString(string.Empty, string.Empty, string.Empty, q, category, radius, scope, maxresults, personid);

            using (ProxomoWebRequest<List<Location>> p = new ProxomoWebRequest<List<Location>>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }

        }

        #endregion

        #region Location AppData Methods

        public string LocationAppDataAdd(string locationID, AppData aData)
        {
            string url = string.Format("{0}/location/{1}/appdata", baseURL, locationID);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "POST", contentType, Converter.Convert(aData, Format, false));
            }
        }

        public void LocationAppDataDelete(string locationID, string appDataID)
        {
            string url = string.Format("{0}/location/{1}/appdata/{2}", baseURL, locationID, appDataID);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                p.GetData(url, "DELETE", contentType);
            }
        }

        public AppData LocationAppDataGet(string locationID, string appDataID)
        {
            string url = string.Format("{0}/location/{1}/appdata/{2}", baseURL, locationID, appDataID);

            using (ProxomoWebRequest<AppData> p = new ProxomoWebRequest<AppData>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }

        public List<AppData> LocationAppDataGetAll(string locationID)
        {
            string url = string.Format("{0}/location/{1}/appdata", baseURL, locationID);

            using (ProxomoWebRequest<List<AppData>> p = new ProxomoWebRequest<List<AppData>>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }

        public void LocationAppDataUpdate(string locationID, AppData aData)
        {
            string url = string.Format("{0}/location/{1}/appdata", baseURL, locationID);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                p.GetData(url, "PUT", contentType, Converter.Convert(aData, Format, false));
            }
        }

        #endregion

        #region    Notification Methods

        public void NotificationSend(Notification notification)
        {
            string url = string.Format("{0}/notification", baseURL);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                p.GetData(url, "POST", contentType, Converter.Convert(notification, Format, false));
            }
        }

        #endregion

        #region    Person Methods

        public Person PersonGet(string personID)
        {
            string url = string.Format("{0}/person/{1}", baseURL, personID);

            using (ProxomoWebRequest<Person> p = new ProxomoWebRequest<Person>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", string.Empty);
            }
        }
        public void PersonUpdate(Person person)
        {
            string url = string.Format("{0}/person", baseURL);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                p.GetData(url, "PUT", contentType, Converter.Convert(person, Format, false));
            }
        }

        public string PersonAppDataAdd(string personID, AppData aData)
        {
            string url = string.Format("{0}/person/{1}/appdata", baseURL, personID);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "POST", contentType, Converter.Convert(aData, Format, false));
            }
        }
        public AppData PersonAppDataGet(string personID, string appDataID)
        {
            string url = string.Format("{0}/person/{1}/appdata/{2}", baseURL, personID, appDataID);

            using (ProxomoWebRequest<AppData> p = new ProxomoWebRequest<AppData>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }
        public List<AppData> PersonAppDataGetAll(string personID)
        {
            string url = string.Format("{0}/person/{1}/appdata", baseURL, personID);

            using (ProxomoWebRequest<List<AppData>> p = new ProxomoWebRequest<List<AppData>>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }
        public void PersonAppDataDelete(string personID, string appDataID)
        {
            string url = string.Format("{0}/person/{1}/appdata/{2}", baseURL, personID, appDataID);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                p.GetData(url, "DELETE", contentType);
            }
        }

        public void PersonAppDataUpdate(string personID, AppData aData)
        {
            string url = string.Format("{0}/person/{1}/appdata", baseURL, personID);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                p.GetData(url, "PUT", contentType, Converter.Convert(aData, Format, false));
            }
        }
        public List<Location> PersonLocationsGet(string personID, string latitude = "", string longitude = "", double radius = 25, int maxresults = 25)
        {
            string url = string.Format("{0}/person/{1}/locations", baseURL, personID) + Utility.FormatQueryString(string.Empty, latitude, longitude, string.Empty, string.Empty, radius, LocationSearchScope.ApplicationOnly, maxresults, personID);

            using (ProxomoWebRequest<List<Location>> p = new ProxomoWebRequest<List<Location>>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }

        }
        public List<SocialNetworkInfo> PersonSocialNetworkInfoGet(string personID, SocialNetwork socialNetwork)
        {
            string url = string.Format("{0}/person/{1}/socialnetworkinfo/socialnetwork/{2}", baseURL, personID, Convert.ToInt16(socialNetwork));

            using (ProxomoWebRequest<List<SocialNetworkInfo>> p = new ProxomoWebRequest<List<SocialNetworkInfo>>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }

        #endregion

        #region  Security Methods

        public PersonLogin SecurityPersonCreate(string userName, string password, string role)
        {
            string url = string.Format("{0}/security/person/create?username={1}&password={2}&role={3}", baseURL, userName, password, role);

            using (ProxomoWebRequest<PersonLogin> p = new ProxomoWebRequest<PersonLogin>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "POST", contentType);
            }
        }

        public void SecurityPersonDelete(string personID)
        {
            string url = string.Format("{0}/security/person/{1}", baseURL, personID);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                p.GetData(url, "DELETE", contentType);
            }
        }

        public List<PersonLogin> SecurityPersonsGetAll()
        {
            string url = string.Format("{0}/security/persons", baseURL);

            using (ProxomoWebRequest<List<PersonLogin>> p = new ProxomoWebRequest<List<PersonLogin>>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }

        public UserToken SecurityPersonAuthenticate(string userName, string password)
        {
            string url = string.Format("{0}/security/person/authenticate?applicationid={1}&username={2}&password={3}", baseURL, ApplicationID, userName, password);

            using (ProxomoWebRequest<UserToken> p = new ProxomoWebRequest<UserToken>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }

        public string SecurityPersonPasswordChangeRequest(string userName)
        {
            string url = string.Format("{0}/security/person/passwordchange/request/{1}", baseURL, userName);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }

        public PersonLogin SecurityPersonPasswordChange(string userName, string password, string resetToken)
        {
            string url = string.Format("{0}/security/person/passwordchange?username={1}&password={2}&resettoken={3}", baseURL, userName, password, resetToken);

            using (ProxomoWebRequest<PersonLogin> p = new ProxomoWebRequest<PersonLogin>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }

        public void SecurityPersonUpdateRole(string personID, string role)
        {
            string url = string.Format("{0}/security/person/update/role?personid={1}&role={2}", baseURL, personID, role);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                p.GetData(url, "PUT", contentType);
            }
        }

        #endregion

        #region Task Methods

        public string TaskAdd(Task task)
        {
            string url = string.Format("{0}/task", baseURL);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "POST", contentType, Converter.Convert(task, Format, false));
            }
        }

        public void TaskDelete(string taskID)
        {
            string url = string.Format("{0}/task/{1}", baseURL, taskID);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                p.GetData(url, "DELETE", contentType);
            }
        }

        public Task TaskGet(string taskID)
        {
            string url = string.Format("{0}/task/{1}", baseURL, taskID);

            using (ProxomoWebRequest<Task> p = new ProxomoWebRequest<Task>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }

        public void TaskUpdate(Task task)
        {
            string url = string.Format("{0}/task", baseURL);

            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                p.GetData(url, "PUT", contentType, Converter.Convert(task, Format, false));
            }
        }

        public List<Task> TasksGetAll()
        {
            string url = string.Format("{0}/tasks", baseURL);

            using (ProxomoWebRequest<List<Task>> p = new ProxomoWebRequest<List<Task>>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }

        public List<Task> TasksGetByPersonID(string personID)
        {
            string url = string.Format("{0}/tasks/person/{1}", baseURL, personID);

            using (ProxomoWebRequest<List<Task>> p = new ProxomoWebRequest<List<Task>>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                return p.GetData(url, "GET", contentType);
            }
        }

        #endregion

    }
}
