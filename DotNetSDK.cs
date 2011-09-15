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

        public ProxomoApi(string applicationID, string proxomoAPIKey, CommunicationType format = CommunicationType.JSON, bool validatessl = true, string url = "")
        {
            Init(applicationID, proxomoAPIKey, "v09", format, validatessl, url);
        }

        public ProxomoApi(string applicationID, string proxomoAPIKey, string version, CommunicationType format = CommunicationType.JSON, bool validatessl = true, string url = "")
        {
            Init(applicationID, proxomoAPIKey, version, format, validatessl, url);
        }

        private void Init(string applicationID, string proxomoAPIKey, string version, CommunicationType format, bool validatessl, string url)
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

            GetAuthToken();
        }

        private void GetAuthToken()
        {
            string url = string.Format("{0}/security/accesstoken/get?applicationid={1}&proxomoAPIKey={2}", baseURL, HttpUtility.UrlEncode(_applicationID), HttpUtility.UrlEncode(_proxomoAPIKey));

            using (ProxomoWebRequest<Token> p = new ProxomoWebRequest<Token>(ValidateSSLCert, Format))
            {
                //AuthToken = p.GetData(url, "GET", string.Empty);
                //AuthToken.ExpiresDate = Utility.ConvertFromUnixTimestamp(AuthToken.Expires);
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

        //public delegate void AppDataAddEventHandler(object e);
        //public event AppDataAddEventHandler AppDataAdd_Complete;

        public void AppDataAdd(AppData appData)
        {
            string url = string.Format("{0}/appdata", baseURL);
                        
            using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
            {
                //  **************    Replace NULL with Callback  *********************
                p.GetDataItem(url, "POST", contentType, Converter.Convert(appData, Format, false), null);
            }
        }

        //public void AppDataDelete(string appDataID)
        //{
        //    string url = string.Format("{0}/appdata/{1}", baseURL, appDataID);

        //    using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        p.GetData(url, "DELETE", contentType);
        //    }
        //}

        //public void AppDataSave(AppData appData)
        //{
        //    string url = string.Format("{0}/appdata", baseURL);

        //    using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        p.GetData(url, "PUT", contentType, Converter.Convert(appData, Format, false));
        //    }
        //}

        //public AppData AppDataGet(string appDataID)
        //{
        //    string url = string.Format("{0}/appdata/{1}", baseURL, appDataID);

        //    using (ProxomoWebRequest<AppData> p = new ProxomoWebRequest<AppData>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "GET", contentType);
        //    }
        //}

        //public List<AppData> AppDataGetByObjectType(string objectType)
        //{
        //    string url = string.Format("{0}/appdata/search/objecttype/{1}", baseURL, objectType);

        //    using (ProxomoWebRequest<List<AppData>> p = new ProxomoWebRequest<List<AppData>>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "GET", contentType);
        //    }
        //}

        //public List<AppData> AppDataGetAll()
        //{
        //    string url = string.Format("{0}/appdata", baseURL);

        //    using (ProxomoWebRequest<List<AppData>> p = new ProxomoWebRequest<List<AppData>>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "GET", contentType);
        //    }
        //}

        //#endregion

        //#region    Event Methods

        //public string EventAdd(Event evt)
        //{
        //    string url = string.Format("{0}/event", baseURL);

        //    using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "POST", contentType, Converter.Convert(evt, Format, false));
        //    }
        //}

        //public void EventSave(Event evt)
        //{
        //    string url = string.Format("{0}/event", baseURL);

        //    using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        p.GetData(url, "PUT", contentType, Converter.Convert(evt, Format, false));
        //    }
        //}

        //public Event EventGet(string eventID)
        //{
        //    string url = string.Format("{0}/event/{1}", baseURL, eventID);

        //    using (ProxomoWebRequest<Event> p = new ProxomoWebRequest<Event>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "GET", contentType);
        //    }
        //}

        //public List<Event> EventsGetByPerson(string personID, DateTime starttime, DateTime endtime, string eventType = "")
        //{
        //    string url = null;

        //    if (string.IsNullOrWhiteSpace(eventType))
        //    {
        //        url = string.Format("{0}/events/search/personid/{1}/start/{2}/end/{3}", baseURL, personID, Utility.ConvertToUnixTimestamp(starttime), Utility.ConvertToUnixTimestamp(endtime));
        //    }
        //    else
        //    {
        //        url = string.Format("{0}/events/search/personid/{1}/start/{2}/end/{3}?eventtype={4}", baseURL, personID, Utility.ConvertToUnixTimestamp(starttime), Utility.ConvertToUnixTimestamp(endtime), eventType);
        //    }

        //    using (ProxomoWebRequest<List<Event>> p = new ProxomoWebRequest<List<Event>>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "GET", contentType);
        //    }
        //}

        //public List<Event> EventsGetByDistance(decimal latitude, decimal longitude, decimal distance, DateTime starttime, DateTime endtime, string eventType = "")
        //{
        //    string url = null;

        //    if (string.IsNullOrWhiteSpace(eventType))
        //    {
        //        url = string.Format("{0}/events/search/latitude/{1}/longitude/{2}/distance/{3}/start/{4}/end/{5}", baseURL, latitude, longitude, distance, Utility.ConvertToUnixTimestamp(starttime), Utility.ConvertToUnixTimestamp(endtime));
        //    }
        //    else
        //    {
        //        url = string.Format("{0}/events/search/latitude/{1}/longitude/{2}/distance/{3}/start/{4}/end/{5}?eventtype={6}", baseURL, latitude, longitude, distance, Utility.ConvertToUnixTimestamp(starttime), Utility.ConvertToUnixTimestamp(endtime), eventType);
        //    }

        //    using (ProxomoWebRequest<List<Event>> p = new ProxomoWebRequest<List<Event>>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "GET", contentType);
        //    }
        //}

        //#endregion

        //#region    EventComment Methods

        //public string EventCommentAdd(string eventID, EventComment comment)
        //{
        //    string url = string.Format("{0}/event/{1}/comment", baseURL, eventID);

        //    using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "POST", contentType, Converter.Convert(comment, Format, false));
        //    }
        //}

        //public List<EventComment> EventCommentsGet(string eventID)
        //{
        //    string url = string.Format("{0}/event/{1}/comments", baseURL, eventID);

        //    using (ProxomoWebRequest<List<EventComment>> p = new ProxomoWebRequest<List<EventComment>>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "GET", contentType);
        //    }
        //}

        //public void EventCommentSave(string eventID, EventComment comment)
        //{
        //    string url = string.Format("{0}/event/{1}/comment", baseURL, eventID);

        //    using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        p.GetData(url, "PUT", contentType, Converter.Convert(comment, Format, false));
        //    }
        //}

        //public void EventCommentDelete(string eventID, string commentID)
        //{
        //    string url = string.Format("{0}/event/{1}comment/{2}", baseURL, eventID, commentID);

        //    using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        p.GetData(url, "DELETE", contentType);
        //    }
        //}

        //#endregion

        //#region    EventParticiapant Methods

        //public string EventParticipantUpdate(string eventID, EventParticipant evtp)
        //{
        //    string url = string.Format("{0}/event/{1}/participant", baseURL, eventID);

        //    using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "PUT", contentType, Converter.Convert(evtp, Format, false));
        //    }
        //}

        //public List<EventParticipant> EventGetParticipants(string eventID)
        //{
        //    string url = string.Format("{0}/event/{1}/participants", baseURL, eventID);

        //    using (ProxomoWebRequest<List<EventParticipant>> p = new ProxomoWebRequest<List<EventParticipant>>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "GET", contentType);
        //    }
        //}

        //public void EventDeleteParticipants(string eventID, string participantID)
        //{
        //    string url = string.Format("{0}/event/{1}/participant/{2}", baseURL, eventID, participantID);

        //    using (ProxomoWebRequest<List<EventParticipant>> p = new ProxomoWebRequest<List<EventParticipant>>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        p.GetData(url, "DELETE", contentType);
        //    }
        //}

        //public void EventPersonInvite(string eventID, string personID)
        //{
        //    string url = string.Format("{0}/event/{1}/participant/invite/personid/{2}", baseURL, eventID, personID);

        //    using (ProxomoWebRequest<EventParticipant> p = new ProxomoWebRequest<EventParticipant>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        p.GetData(url, "PUT", contentType);
        //    }
        //}

        //public void EventPersonsInvite(string eventID, string[] personIDs)
        //{

        //    var personIDstoStrArray = string.Join(",", personIDs);

        //    string url = string.Format("{0}/event/{1}/participants/invite/personids/{2}", baseURL, eventID, personIDstoStrArray);

        //    using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        p.GetData(url, "PUT", contentType);
        //    }

        //}

        //public void EventRequestInvite(string eventID, string personID)
        //{
        //    string url = string.Format("{0}/event/{1}/requestinvite/personid/{2}", baseURL, eventID, personID);

        //    using (ProxomoWebRequest<EventParticipant> p = new ProxomoWebRequest<EventParticipant>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        p.GetData(url, "PUT", contentType);
        //    }
        //}

        //public void EventRSVP(string eventID, EventParticipantStatus participantStatus, string personID)
        //{
        //    string url = string.Format("{0}/event/{1}/rsvp/personid/{2}/participantstatus/{3}", baseURL, eventID, personID, Convert.ToInt16(participantStatus));

        //    using (ProxomoWebRequest<EventParticipant> p = new ProxomoWebRequest<EventParticipant>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        p.GetData(url, "PUT", contentType);
        //    }
        //}

        //#endregion

        //#region    Friends Methods

        //public void FriendshipRespond(FriendResponse response, string friendA, string friendB)
        //{
        //    string url = string.Format("{0}/friend/respond/frienda/{1}/friendb/{2}/friendresponse/{3}", baseURL, friendA, friendB, Convert.ToInt16(response));

        //    using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        p.GetData(url, "PUT", contentType);
        //    }
        //}

        //public List<Friend> FriendsGet(string personID)
        //{
        //    string url = string.Format("{0}/friends/personid/{1}", baseURL, personID);

        //    using (ProxomoWebRequest<List<Friend>> p = new ProxomoWebRequest<List<Friend>>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "GET", contentType);
        //    }
        //}

        //public List<SocialNetworkFriend> FriendsGetBySocialNetwork(SocialNetwork socialNetwork, string personID)
        //{

        //    string url = string.Format("{0}/friends/personid/{1}/socialnetwork/{2}", baseURL, personID, Convert.ToInt16(socialNetwork));

        //    using (ProxomoWebRequest<List<SocialNetworkFriend>> p = new ProxomoWebRequest<List<SocialNetworkFriend>>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "GET", contentType);
        //    }
        //}

        //public List<SocialNetworkPFriend> FriendsGetAppUsersBySocialNetwork(SocialNetwork socialNetwork, string personID)
        //{
        //    string url = string.Format("{0}/friends/app/personid/{1}/socialnetwork/{2}", baseURL, personID, Convert.ToInt16(socialNetwork));

        //    using (ProxomoWebRequest<List<SocialNetworkPFriend>> p = new ProxomoWebRequest<List<SocialNetworkPFriend>>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "GET", contentType);
        //    }
        //}

        //public void FriendInvite(string friendA, string friendB)
        //{
        //    string url = string.Format("{0}/friend/invite/frienda/{1}/friendb/{2}", baseURL, friendA, friendB);

        //    using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        p.GetData(url, "PUT", contentType);
        //    }
        //}

        //public void FriendInviteBySocialNetwork(SocialNetwork socialnetwork, string frienda, string friendb)
        //{
        //    string url = string.Format("{0}/friend/invite/frienda/{1}/friendb/{2}/socialnetwork/{3}", baseURL, frienda, friendb, Convert.ToInt16(socialnetwork));

        //    using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        p.GetData(url, "PUT", contentType);
        //    }
        //}

        //#endregion

        //#region    Location Methods

        //public string LocationAdd(Location location)
        //{
        //    string url = string.Format("{0}/location", baseURL);

        //    using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "POST", contentType, Converter.Convert(location, Format, false));
        //    }
        //}

        //public Location LocationGet(string locationID)
        //{
        //    string url = string.Format("{0}/location/{1}", baseURL, locationID);

        //    using (ProxomoWebRequest<Location> p = new ProxomoWebRequest<Location>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "GET", contentType);
        //    }

        //}

        //public void LocationSave(Location location)
        //{
        //    string url = string.Format("{0}/location", baseURL);

        //    using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        p.GetData(url, "PUT", contentType, Converter.Convert(location, Format, false));
        //    }
        //}

        //public void LocationDelete(string locationID)
        //{
        //    string url = string.Format("{0}/location/{1}", baseURL, locationID);

        //    using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        p.GetData(url, "DELETE", contentType);
        //    }
        //}

        //public List<Category> LocationCategory()
        //{
        //    string url = string.Format("{0}/location/categories", baseURL);

        //    using (ProxomoWebRequest<List<Category>> p = new ProxomoWebRequest<List<Category>>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "GET", contentType);
        //    }
        //}

        //public List<Location> LocationSearchLatLon(string latitude, string longitude, string q = "", string category = "", double radius = 2, LocationSearchScope scope = LocationSearchScope.ApplicationOnly, int maxresults = 10, string personid = "")
        //{
        //    string url = string.Format("{0}/locations/search/latitude/{1}/longitude/{2}", baseURL, latitude, longitude) + Utility.FormatQueryString(string.Empty, string.Empty, string.Empty, q, category, radius, scope, maxresults, personid);

        //    using (ProxomoWebRequest<List<Location>> p = new ProxomoWebRequest<List<Location>>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "GET", contentType);
        //    }

        //}

        //public List<Location> LocationSearchAddress(string address, string q = "", string category = "", double radius = 2, LocationSearchScope scope = LocationSearchScope.ApplicationOnly, int maxresults = 10, string personid = "")
        //{
        //    string url = string.Format("{0}/locations/search", baseURL) + Utility.FormatQueryString(address, string.Empty, string.Empty, q, category, radius, scope, maxresults, personid);

        //    using (ProxomoWebRequest<List<Location>> p = new ProxomoWebRequest<List<Location>>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "GET", contentType);
        //    }

        //}

        //public List<Location> LocationSearchIP(string ipAddress, string q = "", string category = "", double radius = 2, LocationSearchScope scope = LocationSearchScope.ApplicationOnly, int maxresults = 10, string personid = "")
        //{
        //    string url = string.Format("{0}/locations/search/ip/{1}", baseURL, ipAddress) + Utility.FormatQueryString(string.Empty, string.Empty, string.Empty, q, category, radius, scope, maxresults, personid);

        //    using (ProxomoWebRequest<List<Location>> p = new ProxomoWebRequest<List<Location>>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "GET", contentType);
        //    }

        //}

        //public GeoCode Geocode(string address)
        //{
        //    string url = string.Format("{0}/geo/lookup/address/{1}", baseURL, HttpUtility.UrlEncode(address));

        //    using (ProxomoWebRequest<GeoCode> p = new ProxomoWebRequest<GeoCode>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "GET", contentType);
        //    }

        //}

        //public Location ReverseGeocode(string latitude, string longitude)
        //{
        //    string url = string.Format("{0}/geo/lookup/latitude/{1}/longitude/{2}", baseURL, latitude, longitude);

        //    using (ProxomoWebRequest<Location> p = new ProxomoWebRequest<Location>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "GET", contentType);
        //    }
        //}

        //public GeoIP GeoIPGet(string ipAddress)
        //{
        //    string url = string.Format("{0}/geo/lookup/ip/{1}", baseURL, ipAddress);

        //    using (ProxomoWebRequest<GeoIP> p = new ProxomoWebRequest<GeoIP>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "GET", contentType);
        //    }
        //}

        //public string LocationAppDataAdd(string locationID, AppData aData)
        //{
        //    string url = string.Format("{0}/location/{1}/appdata", baseURL, locationID);

        //    using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "POST", contentType, Converter.Convert(aData, Format, false));
        //    }
        //}

        //public void LocationAppDataDelete(string locationID, string appDataID)
        //{
        //    string url = string.Format("{0}/location/{1}/appdata/{2}", baseURL, locationID, appDataID);

        //    using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        p.GetData(url, "DELETE", contentType);
        //    }
        //}

        //public AppData LocationAppDataGet(string locationID, string appDataID)
        //{
        //    string url = string.Format("{0}/location/{1}/appdata/{2}", baseURL, locationID, appDataID);

        //    using (ProxomoWebRequest<AppData> p = new ProxomoWebRequest<AppData>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "GET", contentType);
        //    }
        //}

        //public List<AppData> LocationAppDataGetAll(string locationID)
        //{
        //    string url = string.Format("{0}/location/{1}/appdata", baseURL, locationID);

        //    using (ProxomoWebRequest<List<AppData>> p = new ProxomoWebRequest<List<AppData>>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "GET", contentType);
        //    }
        //}

        //public void LocationAppDataSave(string locationID, AppData aData)
        //{
        //    string url = string.Format("{0}/location/{1}/appdata", baseURL, locationID);

        //    using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        p.GetData(url, "PUT", contentType, Converter.Convert(aData, Format, false));
        //    }
        //}

        //#endregion

        //#region    Notification Methods

        //public void NotificationSend(Notification notification)
        //{
        //    string url = string.Format("{0}/notification", baseURL);

        //    using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        p.GetData(url, "POST", contentType, Converter.Convert(notification, Format, false));
        //    }
        //}

        //#endregion

        //#region    Person Methods

        //public Person PersonGet(string personID)
        //{
        //    string url = string.Format("{0}/person/{1}", baseURL, personID);

        //    using (ProxomoWebRequest<Person> p = new ProxomoWebRequest<Person>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "GET", string.Empty);
        //    }
        //}

        //public void PersonSave(Person person)
        //{
        //    string url = string.Format("{0}/person", baseURL);

        //    using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        p.GetData(url, "PUT", contentType, Converter.Convert(person, Format, false));
        //    }
        //}

        //public string PersonAppDataAdd(string personID, AppData aData)
        //{
        //    string url = string.Format("{0}/person/{1}/appdata", baseURL, personID);

        //    using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "POST", contentType, Converter.Convert(aData, Format, false));
        //    }
        //}

        //public AppData PersonAppDataGet(string personID, string appDataID)
        //{
        //    string url = string.Format("{0}/person/{1}/appdata/{2}", baseURL, personID, appDataID);

        //    using (ProxomoWebRequest<AppData> p = new ProxomoWebRequest<AppData>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "GET", contentType);
        //    }
        //}

        //public List<AppData> PersonAppDataGetAll(string personID)
        //{
        //    string url = string.Format("{0}/person/{1}/appdata", baseURL, personID);

        //    using (ProxomoWebRequest<List<AppData>> p = new ProxomoWebRequest<List<AppData>>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "GET", contentType);
        //    }
        //}

        //public void PersonAppDataDelete(string personID, string appDataID)
        //{
        //    string url = string.Format("{0}/person/{1}/appdata/{2}", baseURL, personID, appDataID);

        //    using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        p.GetData(url, "DELETE", contentType);
        //    }
        //}

        //public void PersonAppDataSave(string personID, AppData aData)
        //{
        //    string url = string.Format("{0}/person/{1}/appdata", baseURL, personID);

        //    using (ProxomoWebRequest<string> p = new ProxomoWebRequest<string>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        p.GetData(url, "PUT", contentType, Converter.Convert(aData, Format, false));
        //    }
        //}

        //public List<SocialNetworkInfo> PersonSocialNetworkInfoGet(string personID, SocialNetwork socialNetwork)
        //{
        //    string url = string.Format("{0}/person/{1}/socialnetworkinfo/socialnetwork/{2}", baseURL, personID, Convert.ToInt16(socialNetwork));

        //    using (ProxomoWebRequest<List<SocialNetworkInfo>> p = new ProxomoWebRequest<List<SocialNetworkInfo>>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "GET", contentType);
        //    }
        //}

        //public List<Location> LocationSearchPersonID(string personID, string latitude = "", string longitude = "", double radius = 25, int maxresults = 25)
        //{
        //    string url = string.Format("{0}/person/{1}/locations", baseURL, personID) + Utility.FormatQueryString(string.Empty, latitude, longitude, string.Empty, string.Empty, radius, LocationSearchScope.ApplicationOnly, maxresults, personID);

        //    using (ProxomoWebRequest<List<Location>> p = new ProxomoWebRequest<List<Location>>(AuthToken.AccessToken, ValidateSSLCert, Format))
        //    {
        //        return p.GetData(url, "GET", contentType);
        //    }

        //}

        #endregion

    }
}