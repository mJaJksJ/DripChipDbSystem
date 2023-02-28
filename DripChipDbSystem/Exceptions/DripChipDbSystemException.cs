using System;

namespace DripChipDbSystem.Exceptions
{
    /// <inheritdoc/>
    public abstract class DripChipDbSystemException : Exception
    {
        /// <summary>
        /// Сообщение ошибки (рус)
        /// </summary>
        public string RussianMessage { get; }

        /// <summary>
        /// .ctor
        /// </summary>
        protected DripChipDbSystemException(string russianMessage = null) : base()
        {
            RussianMessage = russianMessage;
        }

        /// <inheritdoc/>
        public override string Message => $"DripChipDbSystem process error: {RussianMessage}";
    }
}
