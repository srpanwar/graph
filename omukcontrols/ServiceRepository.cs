using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Omuk.OmukControls
{
    /// <summary>
    /// Summary description for ServiceRepository
    /// </summary>
    public class ServiceRepository
    {
        private static ServiceRepository _repo = null;
        private static Dictionary<String, OmukControl[]> senseTypes = new Dictionary<string, OmukControl[]>();
        private ServiceRepository()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// 
        /// </summary>
        public static ServiceRepository Instance
        {
            get
            {
                if (_repo == null)
                    _repo = new ServiceRepository();
                return _repo;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sense"></param>
        /// <returns></returns>
        public OmukControl[] GetFeeders(String sense)
        {
            List<OmukControl> controlsList = new List<OmukControl>();
            controlsList.Add(new YouTubeControl());
            controlsList.Add(new LastFmControl());
            controlsList.Add(new LyricWikiControl());
            //controlsList.Add(new EventfulControl());
            controlsList.Add(new WikiControl());
            controlsList.Add(new DictionaryControl());
            controlsList.Add(new RTControl());
            controlsList.Add(new TwitterSenseControl());
            return controlsList.ToArray();
        }


    }
}