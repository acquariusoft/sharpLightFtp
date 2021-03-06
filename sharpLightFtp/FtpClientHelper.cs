﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace sharpLightFtp
{
	public static class FtpClientHelper
	{
		private static readonly Type TypeOfFtpFeatures = typeof (FtpFeatures);

		public static FtpFeatures ParseFtpFeatures(IEnumerable<string> messages)
		{
			// Example request & response:
			//Request:	FEAT
			//Response:	211-Features:
			//Response:	 MDTM
			//Response:	 MFMT
			//Response:	 TVFS
			//Response:	 UTF8
			//Response:	 MFF modify;UNIX.group;UNIX.mode;
			//Response:	 MLST modify*;perm*;size*;type*;unique*;UNIX.group*;UNIX.mode*;UNIX.owner*;
			//Response:	 LANG ja-JP;ko-KR;bg-BG;zh-CN;it-IT;zh-TW;ru-RU;en-US*;fr-FR
			//Response:	 REST STREAM
			//Response:	 SIZE
			//Response:	211 End

			var ftpFeatures = FtpFeatures.Unknown;

			var complexEnums = (from name in Enum.GetNames(TypeOfFtpFeatures)
			                    let value = Enum.Parse(TypeOfFtpFeatures,
			                                           name,
			                                           true)
			                    select new
			                    {
				                    Name = name,
				                    Value = (FtpFeatures) value
			                    }).ToList();
			foreach (var message in messages)
			{
				foreach (var complexEnum in complexEnums)
				{
					if (0 <= message.IndexOf(complexEnum.Name,
					                         StringComparison.InvariantCultureIgnoreCase))
					{
						var enumValue = complexEnum.Value;
						ftpFeatures |= enumValue;
						break;
					}
				}
			}

			return ftpFeatures;
		}
	}
}
