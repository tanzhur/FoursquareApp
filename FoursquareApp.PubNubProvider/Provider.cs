using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foursquare.PubNubProvider
{
    public class PubnubDemo
    {
        private static PubnubAPI pubnub = new PubnubAPI(
                "pub-c-a45c5506-1f9b-4c93-ae95-86d2fe39cd91",               // PUBLISH_KEY
                "sub-c-5c37945a-0511-11e3-a3d6-02ee2ddab7fe",               // SUBSCRIBE_KEY
                "sec-c-ZmI3NDU5MGUtYWJiYy00OTNhLTliMTctNTc0NzUwNGUxMGNj",   // SECRET_KEY
                true                                                        // SSL_ON?
            );

        private string channel;

        public PubnubDemo(string newChanal)
        {
            this.channel = newChanal;
            Subscribe();
        }

        //internal void Main()
        //{
        //    // Start the HTML5 Pubnub client
        //    //Process.Start("..\\..\\PubNub-HTML5-Client.html");
        //    System.Threading.Thread.Sleep(2000);

        //    // Publish a sample message to Pubnub
        //    //List<object> publishResult = pubnub.Publish(channel, "Hello Pubnub!");
        //    //Console.WriteLine(
        //    //    "Publish Success: " + publishResult[0].ToString() + "\n" +
        //    //    "Publish Info: " + publishResult[1]
        //    //);

        //    // Show PubNub server time
        //    object serverTime = pubnub.Time();
        //    Console.WriteLine("Server Time: " + serverTime.ToString());

        //    // Subscribe for receiving messages (in a background task to avoid blocking)
        //}
  
        private void Subscribe()
        {
            // Subscribe for receiving messages (in a background task to avoid blocking)
            System.Threading.Tasks.Task t = new System.Threading.Tasks.Task(
                () => pubnub.Subscribe(
                    this.channel,
                    delegate(object message)
                    {
                        Console.WriteLine("Received Message -> '" + message + "'");
                        return true;
                    }));
            t.Start();
        }

        public void PublishMessage(string message)
        {
            pubnub.Publish(this.channel, message);
        }
    }
}
