// <copyright file="IGetNext.cs" company="Ayane">
// Copyright (c) Ayane. All rights reserved.
// </copyright>

namespace CodeAnalyzing
{
    /// <summary>
    /// Interface.
    /// </summary>
    /// <typeparam name="T">Ensures that the signature for the operator includes the containing type, or its type argument.</typeparam>
    internal interface IGetNext<T>
        where T : IGetNext<T>
    {
        /// <summary>
        /// Overrides implemenation of operator ++.
        /// </summary>
        /// <param name="other">Instance to apply operator.</param>
        /// <returns>++.</returns>
        static abstract T operator ++(T other);
    }
}
