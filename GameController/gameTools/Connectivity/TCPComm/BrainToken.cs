using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace GBCore.Connectivity
{
    public class BrainToken
    {
        //private string token;

        public string Token { get; set; }
        /// <summary>
        /// <para>Loads a RxToken from an .xml file named "description.xml", if file contains "EnterTokenHere" for 'Token', an ExceptionRxToken is raised to let the SplashScreen know.</para>
        /// <para>If the file doesn't exist, a random RxToken will be generated and stored in a "description.xml" file</para>
        /// </summary>
        /// <returns></returns>
        public static BrainToken LoadDataFromXML()
        {
            XmlDocument xml = new XmlDocument();
            BrainToken toReturn;
            XmlSerializer xmlSer = null;
            string path = string.Empty;

            try
            {
                string basePath = AppDomain.CurrentDomain.BaseDirectory;
                if (basePath != null)
                    path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "description.xml");
                else
                    path = "description.xml";
                xml.Load(path);

                using (StringReader reader = new StringReader(xml.InnerXml))
                {
                    xmlSer = new XmlSerializer(typeof(BrainToken));

                    toReturn = (BrainToken)xmlSer.Deserialize(reader);
                    if (toReturn.Token == "EnterTokenHere")
                    {
                        toReturn = GenerateRandomToken();
                        toReturn.saveData();
                    }
                }
            }
            catch (FileNotFoundException)
            {
                //If file not found exception is raised, we create a random token and we save it
                toReturn = GenerateRandomToken();
                toReturn.saveData();
            }
            catch (InvalidOperationException)
            {
                if (path != string.Empty)
                {
                    using (StreamReader reader = new StreamReader(path))
                    {
                        string reformatedXML = reader.ReadToEnd().Replace("RxToken", "ZeusToken");
                        using (TextReader textReader = new StringReader(reformatedXML))
                        {
                            if (xmlSer != null)
                                toReturn = xmlSer.Deserialize(textReader) as BrainToken;
                            else
                                toReturn = GenerateRandomToken();
                        }
                    }
                    if (toReturn.Token == "EnterTokenHere")
                        toReturn = GenerateRandomToken();

                    toReturn.saveData();
                }
                else //unknown path for description.xml file, we generate a random one
                {
                    toReturn = GenerateRandomToken();
                    toReturn.saveData();
                }
            }
            catch (XmlException)
            {
                //xml file is corrupted, we generate a random token and save it
                toReturn = GenerateRandomToken();
                toReturn.saveData();
            }
            catch (Exception)
            {
                //somethinf unexpected happened, we generate a random token and save it
                toReturn = GenerateRandomToken();
                toReturn.saveData();
            }
            return toReturn;
        }

        /// <summary>
        /// stores de current token in the description file
        /// </summary>
        private void saveData()
        {
            XmlSerializer xml = new XmlSerializer(typeof(BrainToken));

            var path = AppDomain.CurrentDomain.BaseDirectory;
            if (path != null)
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "description.xml");
            else
                path = "description.xml";

            using (TextWriter writer = new StreamWriter(path))
            {
                xml.Serialize(writer, this);
            }
        }

        /// <summary>
        /// provides a valid new random token
        /// </summary>
        /// <returns></returns>
        private static BrainToken GenerateRandomToken()
        {
            BrainToken toReturn = new BrainToken();
            Random rnd = new Random();
            int num = rnd.Next(0000000, 9999999);
            toReturn = new BrainToken();
            toReturn.Token = "ZeusToken_" + num.ToString().PadLeft(7, '0');

            return toReturn;
        }

        public override string ToString() => Token;
    }
}
