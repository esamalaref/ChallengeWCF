using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace FirstWCFWebService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "FirstService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select FirstService.svc or FirstService.svc.cs at the Solution Explorer and start debugging.

    [ErrorHandlerExAttribute(typeof(GlobalErrorHandler))]  
    public class FirstService : IFirstService
    {        
        public ResultSet ProcessFeed(Feed feed)//PayLoad[] payLoad, int skip,int take,int totalRecords)
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
