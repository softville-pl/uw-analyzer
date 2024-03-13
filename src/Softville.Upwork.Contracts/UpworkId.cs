// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Softville.Upwork.Contracts;

public class UpworkId
{
    public string Uid { get; }
    public string CipherText { get; }

    public UpworkId(string uid, string cipherText)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(uid, nameof(uid));
        Uid = uid;

        ArgumentException.ThrowIfNullOrWhiteSpace(cipherText, nameof(cipherText));
        if (cipherText[0] != '~')
        {
            throw new ArgumentException($"cipherText must starts with '~'. Actual value: {cipherText}",
                nameof(cipherText));
        }

        CipherText = cipherText;
    }

    public override string ToString() => Uid;
}
