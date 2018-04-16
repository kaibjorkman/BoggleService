// Written by Joe Zachary for CS 3500, November 2012
// Revised by Joe Zachary April 2016
// Revised extensively by Joe Zachary April 2017

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace CustomNetworking
{
    /// <summary>
    /// The type of delegate that is called when a StringSocket send has completed.
    /// </summary>
    public delegate void SendCallback(bool wasSent, object payload);

    /// <summary>
    /// The type of delegate that is called when a receive has completed.
    /// </summary>
    public delegate void ReceiveCallback(String s, object payload);

    /// <summary> 
    /// A StringSocket is a wrapper around a Socket.  It provides methods that
    /// asynchronously read lines of text (strings terminated by newlines) and 
    /// write strings. (As opposed to Sockets, which read and write raw bytes.)  
    ///
    /// StringSockets are thread safe.  This means that two or more threads may
    /// invoke methods on a shared StringSocket without restriction.  The
    /// StringSocket takes care of the synchronization.
    /// 
    /// Each StringSocket contains a Socket object that is provided by the client.  
    /// A StringSocket will work properly only if the client refrains from calling
    /// the contained Socket's read and write methods.
    /// 
    /// We can write a string to a StringSocket ss by doing
    /// 
    ///    ss.BeginSend("Hello world", callback, payload);
    ///    
    /// where callback is a SendCallback (see below) and payload is an arbitrary object.
    /// This is a non-blocking, asynchronous operation.  When the StringSocket has 
    /// successfully written the string to the underlying Socket, or failed in the 
    /// attempt, it invokes the callback.  The parameter to the callback is the payload.  
    /// 
    /// We can read a string from a StringSocket ss by doing
    /// 
    ///     ss.BeginReceive(callback, payload)
    ///     
    /// where callback is a ReceiveCallback (see below) and payload is an arbitrary object.
    /// This is non-blocking, asynchronous operation.  When the StringSocket has read a
    /// string of text terminated by a newline character from the underlying Socket, or
    /// failed in the attempt, it invokes the callback.  The parameters to the callback are
    /// a string and the payload.  The string is the requested string (with the newline removed).
    /// </summary>

    public class StringSocket : IDisposable
    {
        // Underlying socket
        private Socket socket;

        // Encoding used for sending and receiving
        private static Encoding encoding = new System.Text.UTF8Encoding();

        // Buffer size for reading incoming bytes
        private const int BUFFER_SIZE = 1024;

        //struct to represent a send Request
        private struct BeginSendRequest
        {
            public string Text { get; set; }
            public SendCallback Callback { get; set; }
            public Object Payload { get; set; }
            
        }

        //struct to represent recieve requests
        private struct BeginRecieveRequest
        {
            public ReceiveCallback Callback { get; set; }
            public object Payload { get; set; }
            public int length { get; set; }
        }

        //Queue's to keep track of requests
        private Queue<BeginSendRequest> sendRequestQueue;
        private Queue<BeginRecieveRequest> receiveRequestQueue;
        private Queue<string> recievedLines = new Queue<string>();

        //byte array
        private byte[] pendingSendBytes = new byte[0];
        private int pendingIndex;


        //for processing strings
        private Decoder decoder = encoding.GetDecoder();
        private StringBuilder incoming;

        // Buffers that will contain incoming bytes and characters
        private byte[] incomingBytes = new byte[BUFFER_SIZE];
        private char[] incomingChars = new char[BUFFER_SIZE];



        /// <summary>
        /// Creates a StringSocket from a regular Socket, which should already be connected.  
        /// The read and write methods of the regular Socket must not be called after the
        /// StringSocket is created.  Otherwise, the StringSocket will not behave properly.  
        /// The encoding to use to convert between raw bytes and strings is also provided.
        /// </summary>
        internal StringSocket(Socket s, Encoding e)
        {
            socket = s;
            encoding = e;
            incoming = new StringBuilder();
            sendRequestQueue = new Queue<BeginSendRequest>();
            receiveRequestQueue = new Queue<BeginRecieveRequest>();
            
            // TODO: Complete implementation of StringSocket
        }

        /// <summary>
        /// Shuts down this StringSocket.
        /// </summary>
        public void Shutdown(SocketShutdown mode)
        {
            socket.Shutdown(mode);
        }

        /// <summary>
        /// Closes this StringSocket.
        /// </summary>
        public void Close()
        {
            socket.Close();
        }

        /// <summary>
        /// We can write a string to a StringSocket ss by doing
        /// 
        ///    ss.BeginSend("Hello world", callback, payload);
        ///    
        /// where callback is a SendCallback (see below) and payload is an arbitrary object.
        /// This is a non-blocking, asynchronous operation.  When the StringSocket has 
        /// successfully written the string to the underlying Socket it invokes the callback.  
        /// The parameters to the callback are true and the payload.
        /// 
        /// If it is impossible to send because the underlying Socket has closed, the callback 
        /// is invoked with false and the payload as parameters.
        ///
        /// This method is non-blocking.  This means that it does not wait until the string
        /// has been sent before returning.  Instead, it arranges for the string to be sent
        /// and then returns.  When the send is completed (at some time in the future), the
        /// callback is called on another thread.
        /// 
        /// This method is thread safe.  This means that multiple threads can call BeginSend
        /// on a shared socket without worrying around synchronization.  The implementation of
        /// BeginSend must take care of synchronization instead.  On a given StringSocket, each
        /// string arriving via a BeginSend method call must be sent (in its entirety) before
        /// a later arriving string can be sent.
        /// </summary>
        public void BeginSend(String s, SendCallback callback, object payload)
        {
            lock (sendRequestQueue)
            {
                BeginSendRequest request = new BeginSendRequest();
                request.Text = s;
                request.Payload = payload;
                request.Callback = callback;
                sendRequestQueue.Enqueue(request);

                if (sendRequestQueue.Count == 1 )
                {
                    ProcessSendQueue();
                }
            }
        }
        
        /// <summary>
        /// Proccess the request in the queue and decode bytes and try to send to socket
        /// </summary>
        private void ProcessSendQueue()
        {
            while (sendRequestQueue.Count > 0)
            {
                pendingSendBytes = encoding.GetBytes(sendRequestQueue.Peek().Text);
                
                if(pendingSendBytes.Length > 0)
                {
                    
                    socket.BeginSend(pendingSendBytes, pendingIndex, pendingSendBytes.Length - pendingIndex, SocketFlags.None, SendBytes, null);
                    break;
                    
                }
                
            }
        }

        private void SendBytes(IAsyncResult result)
        {
            int bytesSent = socket.EndSend(result);

            pendingIndex += bytesSent;

            if (bytesSent == 0)
            {
                lock (sendRequestQueue)
                {
                    BeginSendRequest req = sendRequestQueue.Dequeue();
                    ThreadPool.QueueUserWorkItem(x => req.Callback(false, req.Payload));
                    ProcessSendQueue();
                   
                }
            }
            
            
                
            //check if all bytes have been sent
            if(pendingIndex == pendingSendBytes.Length)
            {
                lock(sendRequestQueue)
                {
                    BeginSendRequest req = sendRequestQueue.Dequeue();
                    ThreadPool.QueueUserWorkItem(x => req.Callback(true, req.Payload));

                    pendingIndex = 0;//reset the index
                    
                    ProcessSendQueue();
                }
                

            }

            else
            { 
                //send it again becuase not all bytes were sent
                socket.BeginSend(pendingSendBytes, pendingIndex, pendingSendBytes.Length - pendingIndex, SocketFlags.None, SendBytes, null);
            }
        }

        /// <summary>
        /// We can read a string from the StringSocket by doing
        /// 
        ///     ss.BeginReceive(callback, payload)
        ///     
        /// where callback is a ReceiveCallback (see below) and payload is an arbitrary object.
        /// This is non-blocking, asynchronous operation.  When the StringSocket has read a
        /// string of text terminated by a newline character from the underlying Socket, it 
        /// invokes the callback.  The parameters to the callback are a string and the payload.  
        /// The string is the requested string (with the newline removed).
        /// 
        /// Alternatively, we can read a string from the StringSocket by doing
        /// 
        ///     ss.BeginReceive(callback, payload, length)
        ///     
        /// If length is negative or zero, this behaves identically to the first case.  If length
        /// is positive, then it reads and decodes length bytes from the underlying Socket, yielding
        /// a string s.  The parameters to the callback are s and the payload
        ///
        /// In either case, if there are insufficient bytes to service a request because the underlying
        /// Socket has closed, the callback is invoked with null and the payload.
        /// 
        /// This method is non-blocking.  This means that it does not wait until a line of text
        /// has been received before returning.  Instead, it arranges for a line to be received
        /// and then returns.  When the line is actually received (at some time in the future), the
        /// callback is called on another thread.
        /// 
        /// This method is thread safe.  This means that multiple threads can call BeginReceive
        /// on a shared socket without worrying around synchronization.  The implementation of
        /// BeginReceive must take care of synchronization instead.  On a given StringSocket, each
        /// arriving line of text must be passed to callbacks in the order in which the corresponding
        /// BeginReceive call arrived.
        /// 
        /// Note that it is possible for there to be incoming bytes arriving at the underlying Socket
        /// even when there are no pending callbacks.  StringSocket implementations should refrain
        /// from buffering an unbounded number of incoming bytes beyond what is required to service
        /// the pending callbacks.
        /// </summary>
        public void BeginReceive(ReceiveCallback callback, object payload, int length = 0)
        {
            lock (receiveRequestQueue)
            {

                BeginRecieveRequest request = new BeginRecieveRequest();
                request.Payload = payload;
                request.Callback = callback;
                request.length = length;
                receiveRequestQueue.Enqueue(request);

                if (receiveRequestQueue.Count == 1)
                {
                    ProcessRecieveQueue();
                }
            }
        }

         /// <summary>
        /// Proccess the request in the queue and decode bytes and try to send to socket
        /// </summary>
        private void ProcessRecieveQueue()
        {
            lock (receiveRequestQueue)
            {
                if (receiveRequestQueue.Count > 0 && recievedLines.Count == 1)
                {

                    BeginRecieveRequest req = receiveRequestQueue.Dequeue();
                    string recievedLine = recievedLines.Dequeue();
                    ThreadPool.QueueUserWorkItem(x => req.Callback(recievedLine, req.Payload));

                }
                if (receiveRequestQueue.Count > 0)
                {

                    socket.BeginReceive(incomingBytes, 0, incomingBytes.Length, SocketFlags.None, RecieveBytes, null);

                }
            }
        }

        private void RecieveBytes(IAsyncResult result)
        {
            // Figure out how many bytes have come in
            int bytesRead = socket.EndReceive(result);

            // If no bytes were received, it means the client closed its side of the socket.
            // Report that to the console and close our socket.
            if (bytesRead == 0)
            {
                lock (receiveRequestQueue)
                {
                    BeginRecieveRequest req = receiveRequestQueue.Dequeue();
                    ThreadPool.QueueUserWorkItem(x => req.Callback(null, req.Payload));
                    socket.Close();
                }
            }

            // Otherwise, decode and display the incoming bytes.  Then request more bytes.
            else
            {
                // Convert the bytes into characters and appending to incoming
                int charsRead = decoder.GetChars(incomingBytes, 0, bytesRead, incomingChars, 0, false);
                incoming.Append(incomingChars, 0, charsRead);
                int givenLength = 0;

               if((givenLength = receiveRequestQueue.Peek().length) > 0)
                {
                    //check if the length of the bytes read is equal to the specified length
                    if(incomingBytes.Length >= givenLength)
                    {
                        //trim down to specified size
                        string line = incoming.ToString(0, givenLength);
                        recievedLines.Enqueue(line);

                        ProcessRecieveQueue();

                    }

                    else
                    {
                        //get more data
                        ProcessRecieveQueue();
                    }
                }
                else
                {
                    string line = incoming.ToString();
                    int indexEnd, indexStart = 0;
                    while((indexEnd =line.IndexOf('\n', indexStart)) > 0)
                    {
                        recievedLines.Enqueue(line.Substring(indexStart, (indexEnd - indexStart)));

                        indexStart = indexEnd + 1;
                    }

                    ProcessRecieveQueue();
                }
               
            }

        }
        /// <summary>
        /// Frees resources associated with this StringSocket.
        /// </summary>
        public void Dispose()
        {
            Shutdown(SocketShutdown.Both);
            Close();
        }
    }
}
