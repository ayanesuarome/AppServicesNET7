// <copyright file="RepeatSequence.cs" company="Ayane">
// Copyright (c) Ayane. All rights reserved.
// </copyright>

namespace CodeAnalyzing.Static_Virtual_Member_Interface
{
    /// <summary>
    /// Class.
    /// </summary>
    public struct RepeatSequence : IGetNext<RepeatSequence>
    {
        /// <summary>
        /// Gets or sets text.
        /// </summary>
        public string Text = new string(Ch, 1);

        private const char Ch = 'A';

        /// <summary>
        /// Initializes a new instance of the <see cref="RepeatSequence"/> struct.
        /// </summary>
        public RepeatSequence()
        {
        }

        /// <summary>
        /// Doc.
        /// </summary>
        /// <param name="other">Other.</param>
        /// <returns>++.</returns>
        public static RepeatSequence operator ++(RepeatSequence other)
            => other with { Text = other.Text + Ch };

        /// <summary>
        /// Tostring.
        /// </summary>
        /// <returns>Text.</returns>
        public override string ToString() => this.Text;
    }
}
