using System;

namespace CountCSharpLines {
	public struct FileLineCount : IComparable<FileLineCount> {
		public string fileName;
		public int lineCount;

		public FileLineCount(string fileName, int lineCount) {
			if (fileName == null)
				throw new ArgumentNullException();
			this.fileName = fileName;
			this.lineCount = Math.Max(0, lineCount);
		}

		public int CompareTo(FileLineCount other) {
			return lineCount - other.lineCount; //Difference towards this one -- + means this belongs after
		}

		public override string ToString() {
			return string.Format("{0, -6} lines were counted from {1}", lineCount, fileName);
		}
	}
}