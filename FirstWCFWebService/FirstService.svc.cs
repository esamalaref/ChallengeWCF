using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace FirstWCFWebService
{
    [ErrorHandlerExAttribute(typeof(GlobalErrorHandler))]  
    public class FirstService : IFirstService
    {        
        public ResultSet ProcessFeed(Feed feed)
        {
            ResultSet res = new ResultSet();
            res.response = feed.payload.Where(element => element.drm == true && element.episodeCount>0)
                .Select(element => new Show
                        {
                            image = element.image.showImage,slug = element.slug,title = element.title        
                        }).ToList();
            return res;
        }       
    }    
}
