using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

using SharpSvn;

namespace BlueGo
{
    /// <summary>
    ///     Enum for repositories to check out source code,
    ///     for instance svn, git.
    /// </summary>
    enum eRepository
    {
        svn
    }

    class DownloadHelper
    {
        /// <summary>
        /// Function to download a file from URL and save it to local drive
        /// </summary>
        /// <param name="_URL">URL address to download file</param>
        static public void DownloadFileFromURL(string _URL, string _SaveAs)
        {
            try
            {
                if(!RemoteFileExists(_URL))
                {
                    throw new Exception("Url " + _URL + " is not valid.");
                }

                System.Net.WebClient _WebClient = new System.Net.WebClient();
                // Downloads the resource with the specified URI to a local file.
                _WebClient.DownloadFile(_URL, _SaveAs);
            }
            catch (Exception exception)
            {
                // Error
                Console.WriteLine("Exception caught in process: {0}", exception.ToString());

                throw new Exception("Download failed." + exception.ToString());
            }
        }

        ///
        /// Checks the file exists or not.
        ///
        /// The URL of the remote file.
        /// True : If the file exits, False if file not exists
        static public bool RemoteFileExists(string url)
        {
            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "GET";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                response.Close();

                //check for request file and response file, so that in case of redirect the response file 
                //is what was requested.
                string requestFile = request.RequestUri.Segments[request.RequestUri.Segments.Length - 1];
                string responseFile = response.ResponseUri.Segments[response.ResponseUri.Segments.Length - 1];

                if (requestFile.Equals(responseFile) || requestFile.Contains(responseFile))
                {
                    //Returns TURE if the Status code == 200
                    return (response.StatusCode == HttpStatusCode.OK);
                }

                return false;
            }
            catch(Exception ex)
            {
                //Any exception will returns false.
                return false;
            }
        }

        /// <summary>
        ///     Method to checkout current source code from the specified
        ///     repository with given url and a destination folder
        /// </summary>
        /// <param name="repo">
        ///     Repository under consideration
        /// </param>
        /// <param name="url">
        ///     URL from where the source code to checkout
        /// </param>
        /// <param name="destinationFolder">
        ///     Folder to save the source code
        /// </param>
        static public void CheckOutFromSourceSVN(string repo, string url, string destinationFolder)
        {
            if(repo.Equals(eRepository.svn.ToString()))
            {
                using (SvnClient client = new SvnClient())
                {
                    SvnUriTarget uriTarget = new SvnUriTarget(url);
                    client.CheckOut(uriTarget, destinationFolder);
                }
            }
        }
    }
}
