using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Diagnostics;

namespace LibAllDebrid
{
   public class AllDebrid
    {

        #region Properties
        private string _login_url = "http://alldebrid.com/api.php";
        private string _get_link_url = "http://www.alldebrid.com/service.php?json=true&link=";
        private string _url_hosters = "https://www.alldebrid.com/api.php?action=get_host";

        public string daysLeft { get; set; }

        public string _cookie { get; set; }
        #endregion

        public AllDebrid(string username, string password)
        {
            Login(username, password);
        }

       public AllDebrid(string cookie)
       {
           _cookie = cookie;
       }

       public AllDebrid()
       {
           
       }

        /// <summary>
        /// Authenticate to alldebrid server.
        /// Si username et password sont corrects, l'attribut privé _cookie est rempli.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>
        /// True, si l'authentification a réussie.
        /// </returns>
        public bool Login(string username, string password)
        {
            using (var client = new WebClient())
            {
                var response =
                    client.DownloadString(_login_url + "?login=" + username + "&pw=" + password + "&action=info_user");
                if (response == "login fail")
                {
                    throw new InvalidCredentials();
                };
                XDocument xml = XDocument.Parse(response.ToString());
                var cookieQuery = xml.Descendants().SingleOrDefault(e => e.Name == "cookie");

                var dateQuery = xml.Descendants().SingleOrDefault(e => e.Name == "date");

                if(dateQuery != null && dateQuery.Value != null){
                    daysLeft = dateQuery.Value;
                    Debug.Write(daysLeft);
                }

                // Le cookie a été trouvé
                if (cookieQuery != null && cookieQuery.Value != null)
                {
                    _cookie = cookieQuery.Value;
                    return true;
                }
                throw new InvalidCredentials();
            }
        }

   

       /// <summary>
        /// Retourne une objet Link si le lien à correctement été débridé.
        /// null sinon.
        /// </summary>
        /// <param name="url">Url à débrider.</param>
        /// <returns></returns>
        public Link GetUrlDebride(string url)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add(HttpRequestHeader.Cookie, "uid=" + _cookie);
                var full_url = _get_link_url + url;
                var response = client.DownloadString(full_url);
                if (response == "Fuck you please.")
                {
                    throw new InvalidCookie();
                }
                Link link = JsonConvert.DeserializeObject<Link>(response);
                if (link.error == "This link isn't available on the hoster website.")
                {
                    throw new InvalidLink();
                }

                if (link != null && link.link != null)
                {
                    return link;
                }
                return null;
            }
        }
           
    }

    public class InvalidLink : Exception
    {
        public InvalidLink()
            : base() {}
    }


   
}
