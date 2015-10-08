using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Omuk.OmukControls
{
    /// <summary>
    /// Summary description for OmukControl
    /// </summary>
    public interface OmukControl
    {
        String Sense { get; }
        String Name { get; }
        String Area { get; }
        String GetHtmlContent(String searchText, String data, String sense, object Source, Dictionary<String, String> uniqueLinks);
    }
}