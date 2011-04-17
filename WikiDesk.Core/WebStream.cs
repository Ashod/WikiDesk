
namespace WikiDesk.Core
{
    using System;
    using System.IO;
    using System.Net;
    using System.Reflection;

    /// <summary>
    /// HTTP Stream, wraps around HttpWebRequest.
    /// </summary>
    public class WebStream : Stream
	{
		public WebStream(string uri)
            : this(uri, 0)
		{
		}

        public WebStream(string uri, long position)
		{
			Uri = uri;
			position_ = position;
		}

        #region properties

        public string Uri { get; protected set; }
        public string UserAgent { get; set; }
        public string Referer { get; set; }

        #endregion // properties

        #region Overrides of Stream

        /// <summary>
        /// When overridden in a derived class, clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public override void Flush()
        {
        }

        /// <summary>
        /// When overridden in a derived class, sets the position within the current stream.
        /// </summary>
        /// <returns>
        /// The new position within the current stream.
        /// </returns>
        /// <param name="offset">A byte offset relative to the <paramref name="origin"/> parameter.</param>
        /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin"/> indicating the reference point used to obtain the new position.</param>
        /// <filterpriority>1</filterpriority>
        /// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// When overridden in a derived class, sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        /// <filterpriority>2</filterpriority>
        /// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// When overridden in a derived class, reads a sequence of bytes from the current stream and advances the position within the stream by the number of bytes read.
        /// </summary>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        /// <param name="buffer">An array of bytes. When this method returns, the buffer contains the specified byte array with the values between <paramref name="offset"/> and (<paramref name="offset"/> + <paramref name="count"/> - 1) replaced by the bytes read from the current source.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin storing the data read from the current stream.</param>
        /// <param name="count">The maximum number of bytes to be read from the current stream.</param>
        /// <filterpriority>1</filterpriority>
        /// <exception cref="System.ArgumentException">The sum of offset and count is larger than the buffer length.</exception>
        /// <exception cref="System.ArgumentNullException">buffer is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">offset or count is negative.</exception>
        /// <exception cref="System.NotSupportedException">The stream does not support reading.</exception>
        /// <exception cref="System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
		public override int Read(byte[] buffer, int offset, int count)
		{
            if (stream_ == null)
            {
                Connect();
            }

            try
            {
                if (stream_ != null)
                {
                    int read = stream_.Read(buffer, offset, count);
                    position_ += read;
                    return read;
                }
            }
            catch (WebException)
            {
                Close();
            }
            catch (IOException)
            {
                Close();
            }

            return -1;
		}

        /// <summary>
        /// When overridden in a derived class, writes a sequence of bytes to the current stream and advances the current position within this stream by the number of bytes written.
        /// </summary>
        /// <param name="buffer">An array of bytes. This method copies <paramref name="count"/> bytes from <paramref name="buffer"/> to the current stream.</param>
        /// <param name="offset">The zero-based byte offset in <paramref name="buffer"/> at which to begin copying bytes to the current stream.</param>
        /// <param name="count">The number of bytes to be written to the current stream.</param>
        /// <filterpriority>1</filterpriority>
        /// <exception cref="NotImplementedException"><c>NotImplementedException</c>.</exception>
        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports reading.
        /// </summary>
        /// <returns>
        /// true if the stream supports reading; otherwise, false.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override bool CanRead
        {
            get { return true; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports seeking.
        /// </summary>
        /// <returns>
        /// true if the stream supports seeking; otherwise, false.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override bool CanSeek
        {
			get { return false; }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether the current stream supports writing.
        /// </summary>
        /// <returns>
        /// true if the stream supports writing; otherwise, false.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public override bool CanWrite
        {
			get { return false; }
        }

        /// <summary>
        /// When overridden in a derived class, gets the length in bytes of the stream.
        /// </summary>
        /// <returns>
        /// A long value representing the length of the stream in bytes.
        /// </returns>
        /// <exception cref="T:System.ObjectDisposedException">Methods were called after the stream was closed.</exception>
        /// <filterpriority>1</filterpriority>
        public override long Length
        {
            get { return webResponse_.ContentLength; }
        }

        /// <summary>
        /// When overridden in a derived class, gets or sets the position within the current stream.
        /// </summary>
        /// <returns>
        /// The current position within the stream.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        /// <exception cref="NotSupportedException"><c>NotSupportedException</c>.</exception>
        public override long Position
        {
			get { return position_; }
			set { throw new NotSupportedException(); }
        }

        #endregion // Overrides of Stream

        #region operations

        /// <summary>
        /// Reads the full string data at the given URI.
        /// </summary>
        /// <returns>The full contents of the given URI.</returns>
        public static string ReadToEnd(string uri, string userAgent, string referer)
        {
            using (WebStream ws = new WebStream(uri, 0))
            {
                ws.UserAgent = userAgent;
                ws.Referer = referer;
                ws.Connect();

                using (StreamReader reader = new StreamReader(ws.stream_))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Writes the full data at the given URI to the given stream.
        /// </summary>
        /// <returns>The number of bytes written.</returns>
        public static long WriteToStream(string uri, string userAgent, string referer, Stream stream)
        {
            using (WebStream ws = new WebStream(uri, 0))
            {
                ws.UserAgent = userAgent;
                ws.Referer = referer;
                ws.Connect();

                long total = 0;
                byte[] buffer = new byte[64 * 1024];
                int read;
                while ((read = ws.stream_.Read(buffer, 0, buffer.Length)) > 0)
                {
                    stream.Write(buffer, 0, read);
                    total += read;
                }

                return total;
            }
        }

        #endregion // operations

        #region implementation

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (stream_ != null)
            {
                stream_.Dispose();
                stream_ = null;
            }
        }

        private void Connect()
        {
            Close();

            HttpWebRequest request = WebRequest.Create(Uri) as HttpWebRequest;
            if (request == null)
            {
                return;
            }

            request.UserAgent = UserAgent;
            request.Referer = Referer;
            if (position_ > 0)
            {
                SetWebRequestRange(request, position_, 0);
            }

            webResponse_ = request.GetResponse();
            stream_ = webResponse_.GetResponseStream();
        }

        private static void SetWebRequestRange(HttpWebRequest request, long from, long to)
        {
            MethodInfo method = typeof(WebHeaderCollection).GetMethod("AddWithoutValidate", BindingFlags.Instance | BindingFlags.NonPublic);

            string val = string.Format("bytes={0}-{1}", from, to < from ? string.Empty : to.ToString());
            method.Invoke(request.Headers, new object[] { "Range", val });
        }

        #endregion // implementation

        #region representation

        private long position_;
        private WebResponse webResponse_;
        private Stream stream_;

        #endregion // representation
	}
}
