using lzo.net;

using System.IO.Compression;
using System.Security.Cryptography;

namespace ArkhamSaveParser {
	public class DecompressedSave {
		public DecompressedSave(CompressedSave compressedSave) {
			if (compressedSave.TotalDecompressedSize <= 0) {
				throw new InvalidOperationException("Invalid total decompressed size");
			}

			rawData = new byte[compressedSave.TotalDecompressedSize];

			int destOffset = 0;
			foreach (var segment in compressedSave.Segments) {
				var compressedBuffer = compressedSave.ReadFileSegment(segment);
				var compressedMs = new MemoryStream(compressedBuffer);
				var lzo = new LzoStream(compressedMs, CompressionMode.Decompress);
				int totalRead = 0;
				while (totalRead < segment.decompressedSize) {
					int read = lzo.Read(rawData, destOffset + totalRead, segment.decompressedSize - totalRead);
					if (read <= 0) {
						break;
					}

					totalRead += read;
				}

				if (totalRead != segment.decompressedSize) {
					destOffset += totalRead;
					Console.WriteLine("Warning: totalRead was " + totalRead + ", expected " + segment.decompressedSize);
					continue;
				}

				destOffset += segment.decompressedSize;
			}
		}

		public string GetHashString() {
			var totalHash = SHA256.HashData(rawData);
			return Convert.ToHexStringLower(totalHash);
		}

		private byte[] rawData;
	}
}
