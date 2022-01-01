﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Azure;
using Azure.Core;

namespace Fixit.Core.Storage.DataContracts.FileSystem.Models
{
	public class AzureResponseAdapter : Response
	{
		private int _status;

		public AzureResponseAdapter (int status)
		{
			_status = status;
		}

		public override int Status { 
			get { return _status; } 
		}

		public override string ReasonPhrase => throw new NotImplementedException();

		public override Stream ContentStream { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public override string ClientRequestId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public override void Dispose()
		{
			throw new NotImplementedException();
		}

		protected override bool ContainsHeader(string name)
		{
			throw new NotImplementedException();
		}

		protected override IEnumerable<HttpHeader> EnumerateHeaders()
		{
			throw new NotImplementedException();
		}

		protected override bool TryGetHeader(string name, [NotNullWhen(true)] out string value)
		{
			throw new NotImplementedException();
		}

		protected override bool TryGetHeaderValues(string name, [NotNullWhen(true)] out IEnumerable<string> values)
		{
			throw new NotImplementedException();
		}
	}
}
