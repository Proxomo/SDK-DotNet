using System;
using System.Net;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace Proxomo
{

	internal class ProxomoWebRequest<t> : IDisposable
	{

		public string token = string.Empty;
		private bool _validateSSLCert = false;
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

		public ProxomoWebRequest(bool validateSSLCert, CommunicationType format)
		{
			_validateSSLCert = validateSSLCert;
			this.Format = format;
		}

		public ProxomoWebRequest(string authToken, bool validateSSLCert, CommunicationType format)
		{
			token = authToken;
			_validateSSLCert = validateSSLCert;
			this.Format = format;
		}

		internal t GetData(string url, string method, string contentType)
		{
            ContinuationTokens cTokens = new ContinuationTokens(null, null);
            return GetData(url, method, contentType, string.Empty, ref cTokens);
		}

        internal t GetData(string url, string method, string contentType, string content)
        {
            ContinuationTokens cTokens = new ContinuationTokens(null, null);
            return GetData(url, method, contentType, content, ref cTokens);
        }

        internal t GetData(string url, string method, string contentType, string content, ref ContinuationTokens cTokens)
		{
            string NextPartitionKeyDescriptor = "NextPartitionKey";
            string NextRowKeyDescriptor = "NextRowKey";

			WebRequest client = HttpWebRequest.Create(url);
			client.ContentType = contentType;
			client.Method = method;

			if (!(string.IsNullOrEmpty(token)))
			{
				client.Headers.Add("Authorization", token);
			}

            // Include continuation tokens in the request header to the service if they were specified but the caller (i.e. sent in)...
            if (!(string.IsNullOrEmpty(cTokens.NextPartitionKey)) && !(string.IsNullOrEmpty(cTokens.NextPartitionKey)))
            {
                client.Headers.Add(NextPartitionKeyDescriptor, cTokens.NextPartitionKey);
                client.Headers.Add(NextRowKeyDescriptor, cTokens.NextRowKey);
            }


			if (content.Length > 0)
			{
				System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
				byte[] bytes = encoding.GetBytes(content);
				client.ContentLength = bytes.Length;

				using (Stream os = client.GetRequestStream())
				{
					os.Write(bytes, 0, bytes.Length);
				}

				encoding = null;
				bytes = null;
			}
			else
			{
				client.ContentLength = 0;
			}

            if (!_validateSSLCert)
            {                        
                ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(returnTrue);
            }


			using (WebResponse response = client.GetResponse())
			{
                // Return back to caller the continuation tokens returned by the service response (if any) ... 
                cTokens.NextPartitionKey = null;
                cTokens.NextRowKey = null;

                cTokens.NextPartitionKey = response.Headers.Get(NextPartitionKeyDescriptor);
                cTokens.NextRowKey = response.Headers.Get(NextRowKeyDescriptor);
                

				using (Stream resultStream = response.GetResponseStream())
				{
					using (StreamReader sreader = new StreamReader(resultStream))
					{
                        client = null;
						if (this.Format == CommunicationType.XML) 
						{
                            
							return ReturnXML(sreader);
						}
						else if (this.Format == CommunicationType.JSON)
						{
							return ReturnJSON(sreader);
						}
						else
						{
							return default(t);
						}
					}
				}
			}
		}

		private t ReturnXML(StreamReader sreader)
		{
			if (typeof(t).Equals(typeof(string)))
			{
				string result = sreader.ReadToEnd().Replace("<string>", "").Replace("</string>", "");
				return (t)Convert.ChangeType(result, typeof(t));
			}
			else
			{
				DataContractSerializer ds = new DataContractSerializer(typeof(t));
				t result = (t)(ds.ReadObject(sreader.BaseStream));
				ds = null;
				return result;
			}
		}

		private t ReturnJSON(StreamReader sreader)
		{
			if (typeof(t).Equals(typeof(string)))
			{
				string result = sreader.ReadToEnd().Replace("\"", "");
				return (t)Convert.ChangeType(result, typeof(t));
			}
			else
			{
				DataContractJsonSerializer ds = new DataContractJsonSerializer(typeof(t));
				t result = (t)(ds.ReadObject(sreader.BaseStream));
				ds = null;
				return result;
			}
		}

		private bool returnTrue(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
		{
			return true;
		}

		private bool disposedValue;

		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposedValue)
			{
				if (disposing)
				{
					token = null;
				}
			}
			this.disposedValue = true;
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

	}

}
