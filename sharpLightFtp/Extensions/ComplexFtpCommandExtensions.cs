﻿using System;
using System.Diagnostics.Contracts;

namespace sharpLightFtp.Extensions
{
	internal static class ComplexFtpCommandExtensions
	{
		internal static bool Send(this ComplexFtpCommand complexFtpCommand)
		{
			Contract.Requires(complexFtpCommand != null);
			Contract.Requires(!string.IsNullOrWhiteSpace(complexFtpCommand.Command));

			var encoding = complexFtpCommand.Encoding;
			var command = string.Concat(complexFtpCommand.Command, Environment.NewLine);
			var complexSocket = complexFtpCommand.ComplexSocket;
			var endPoint = complexSocket.EndPoint;
			var socket = complexSocket.Socket;

			var sendSocketEventArgs = endPoint.GetSocketEventArgs();
			{
				var sendBuffer = encoding.GetBytes(command);
				sendSocketEventArgs.SetBuffer(sendBuffer, 0, sendBuffer.Length);
			}

			var async = socket.SendAsync(sendSocketEventArgs);
			if (async)
			{
				sendSocketEventArgs.AutoResetEvent.WaitOne();
			}

			var exception = sendSocketEventArgs.ConnectByNameError;

			return exception == null;
		}
	}
}