using System.Buffers.Binary;

namespace ArkhamSaveParser {
	public struct FileSegment(int inOffset, int inCompressedSize, int inDecompressedSize) {
		public long offset = inOffset;
		public int compressedSize = inCompressedSize;
		public int decompressedSize = inDecompressedSize;
	}

	public class CompressedSave {
		public CompressedSave(string filePath) {
			rawData = File.ReadAllBytes(filePath);
			offsets = ExtractSaveOffsets(rawData);

			reader = new BinaryReader(new MemoryStream(rawData));

			segments = [];
			FindSegments();

			if (totalDecompressedSize <= 0) {
				throw new InvalidOperationException("Invalid total decompressed size");
			}
		}

		public byte[] ReadFileSegment(FileSegment segment) {
			reader.BaseStream.Seek(segment.offset, SeekOrigin.Begin);
			var compressedBuffer = new byte[segment.compressedSize];
			reader.Read(compressedBuffer, 0, segment.compressedSize);
			return compressedBuffer;
		}

		public List<FileSegment> Segments { get { return segments; } }
		public int TotalDecompressedSize { get { return totalDecompressedSize; } }

		private byte[] rawData;
		private int[] offsets;
		private List<FileSegment> segments;
		private int totalDecompressedSize;
		private BinaryReader reader;

		private void FindSegments() {
			totalDecompressedSize = 0;
			foreach (var offset in offsets) {
				FileSegment segment;

				reader.BaseStream.Seek(offset, SeekOrigin.Begin);
				//- Header: 0x9e2a83c1 (2653586369)
				_ = ParserUtils.ReadUint32_BE(reader); // header
				_ = ParserUtils.ReadUint32_BE(reader); // + 4, unknown
				segment.compressedSize = (int)ParserUtils.ReadUint32_BE(reader); // + 8
				segment.decompressedSize = (int)ParserUtils.ReadUint32_BE(reader); // + 12
				reader.BaseStream.Position += 8; // skip 8 bytes before compressed data
				segment.offset = reader.BaseStream.Position;
				segments.Add(segment);

				totalDecompressedSize += segment.decompressedSize;
			}

			if (totalDecompressedSize >= int.MaxValue) {
				totalDecompressedSize = -1;
			}
		}

		private static int[] ExtractSaveOffsets(byte[] buffer) {
			var offsets = new List<int>();

			for (int i = 0; i < buffer.Length - 4; i++) {
				if (buffer[i] != 0x9E) {
					continue;
				}

				if (buffer[i + 1] != 0x2A) {
					continue;
				}

				if (buffer[i + 2] != 0x83) {
					continue;
				}

				if (buffer[i + 3] != 0xC1) {
					continue;
				}

				offsets.Add(i);
			}

			return offsets.ToArray();
		}
	}
}
