using System;

namespace DripChipDbSystem.Exceptions
{
    /// <inheritdoc/>
    public class DripChipDbSystemException : Exception
    {
        /// <summary>
        /// Сообщение ошибки (рус)
        /// </summary>
        public string RussianMessage { get; }

        /// <summary>
        /// .ctor
        /// </summary>
        public DripChipDbSystemException(string russianMessage = null)
        {
            RussianMessage = russianMessage;
        }

        /// <inheritdoc/>
        public override string Message => $"DripChipDbSystem process error: {RussianMessage}";
    }
}
