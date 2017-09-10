using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace FirstWCFWebService
{
    
    [ServiceContract]    
    public interface IFirstService
    {
        [OperationContract]        
        [WebInvoke(Method = "POST",
            ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json,
                        UriTemplate = "ProcessFeed")]
        ResultSet ProcessFeed(Feed feed);
    }
    
    [DataContract]
    public class Feed
    {
        [DataMember]
        public List<Elements> payload { get; set; }
        [DataMember]
        public int skip { get; set; }
        [DataMember]
        public int take { get; set; }
        [DataMember]
        public int totalRecords { get; set; }
    }

    [DataContract]
    public class PayLoad
    {
        [DataMember]
        public Elements[] elements{ get; set; }
    }

    [DataContract]
    public class Elements
    {
        [DataMember]
        public string country { get; set; }
        [DataMember]
        public string description { get; set; }
        [DataMember]
        public string genre { get; set; }
        [DataMember]
        public string slug { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string tvChannel { get; set; }
        [DataMember]
        public bool drm { get; set; }
        [DataMember]
        public int episodeCount { get; set; }
        [DataMember]
        public Image image { get; set; }

        //[DataMember]
        //public string[] seasons { get; set; }
        //[DataMember]
        //public NextEpisode nextEpisode { get; set; }
    }

    [DataContract]
    public class Image
    {
        [DataMember]
        public string showImage { get; set; }
    }

    [DataContract]
    public class NextEpisode
    {
        [DataMember]
        public string channel { get; set; }
        [DataMember]
        public string channelLogo { get; set; }
        [DataMember]
        public string date { get; set; }
        [DataMember]
        public string html { get; set; }
        [DataMember]
        public string url { get; set; }
    }

    [DataContract]
    public class ResultSet
    {
        [DataMember]
        public List<Show> response { get; set; }        
    }

    [DataContract]
    public class Show
    {
        [DataMember]
        public string slug { get; set; }
        [DataMember]
        public string title { get; set; }
        [DataMember]
        public string image { get; set; }
    }
}
