namespace WikiDesk.Core
{
    using System.Collections.Generic;

    /// <summary>
    /// Maps a message name to a specific message.
    /// The IDs are short names for the messages.
    /// Typically a default message is overridden by a language-specific version.
    /// </summary>
    public class WikiMessages
    {
        /// <summary>
        /// Finds a message given its name, if registered.
        /// </summary>
        /// <param name="name">A message name to find.</param>
        /// <returns>The message, if found, otherwise null.</returns>
        public string FindMessage(string name)
        {
            string message;

            if (messagesMap_.TryGetValue(name, out message))
            {
                return message;
            }

            // No luck.
            return null;
        }

        /// <summary>
        /// Registers a message with a unique name.
        /// Newer names override older versions.
        /// </summary>
        /// <param name="name">The message name.</param>
        /// <param name="message">The message text.</param>
        public void Register(string name, string message)
        {
            messagesMap_[name] = message;
        }

        #region representation

        private readonly Dictionary<string, string> messagesMap_ = new Dictionary<string, string>(64);

        #endregion // representation
    }
}
