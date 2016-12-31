using System;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.UI;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace SolitaireAI {
	public static class Util {
		public static double CompareHistograms(Mat img, Mat img2) {
			using (Mat hist = new Mat())
			using (Mat hist2 = new Mat())
			using (VectorOfMat vm = new VectorOfMat())
			using (VectorOfMat vm2 = new VectorOfMat()) {
				vm.Push(img);
				vm2.Push(img2);
				var channels = new int[] { 0 };
				var histSize = new int[] { 256 };
				var ranges = new float[] { 0, 256, };
				CvInvoke.CalcHist(vm, channels, null, hist, histSize, ranges, false);
				CvInvoke.CalcHist(vm2, channels, null, hist2, histSize, ranges, false);

				//CvInvoke.Normalize(hist, hist, 0, 255, NormType.MinMax);
				//CvInvoke.Normalize(hist2, hist2, 0, 255, NormType.MinMax);

				//double res = CvInvoke.CompareHist(hist, hist2, HistogramCompMethod.Bhattacharyya);
				//Debug.Log("Cards in Stock: " + (res > 0.5));

				return CvInvoke.CompareHist(hist, hist2, HistogramCompMethod.Correl);
			}
		}

		public static double GetSimilarity(Mat A, Mat B) {
			using (Mat result = new Mat()) {
				CvInvoke.MatchTemplate(A, B, result, TemplateMatchingType.CcoeffNormed);
				double[] minValues, maxValues;
				Point[] minLocations, maxLocations;
				result.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

				return maxValues[0];
			}
		}

		// Compare two images by getting the L2 error (square-root of sum of squared error).
		public static double GetSimilarityL2(Mat A, Mat B) {
			if (A.Rows > 0 && A.Rows == B.Rows && A.Cols > 0 && A.Cols == B.Cols) {
				// Calculate the L2 relative error between images.
				double errorL2 = CvInvoke.Norm(A, B, NormType.L2);
				// Convert to a reasonable scale, since L2 error is summed across all pixels of the image.
				double similarity = errorL2 / (double)(A.Rows * A.Cols);
				return similarity;
			}
			else {
				//Images have a different size
				return 100000000.0;  // Return a bad value
			}
		}

		/*public static double Correlation(Mat image_1, Mat image_2) {
			// convert data-type to "float"
			Mat im_float_1 = new Mat();
			image_1.ConvertTo(im_float_1, DepthType.Cv32F);
			Mat im_float_2 = new Mat();
			image_2.ConvertTo(im_float_2, DepthType.Cv32F);

			int n_pixels = im_float_1.Rows * im_float_1.Cols;

			// Compute mean and standard deviation of both images
			MCvScalar im1_Mean = new MCvScalar(), im1_Std = new MCvScalar(), im2_Mean = new MCvScalar(), im2_Std = new MCvScalar();
			CvInvoke.MeanStdDev(im_float_1, ref im1_Mean, ref im1_Std);
			CvInvoke.MeanStdDev(im_float_2, ref im2_Mean, ref im2_Std);

			// Compute covariance and correlation coefficient
			
			double covar = (im_float_1 - im1_Mean).dot(im_float_2 - im2_Mean) / n_pixels;
			double correl = covar / (im1_Std[0] * im2_Std[0]);

			return correl;
		}*/

		public static Mat ByteArrayToMat(byte[] data, Size size, int stride) {
			GCHandle handle = GCHandle.Alloc(data, GCHandleType.Pinned);
			try {
				using (Mat img = new Mat(size, DepthType.Cv8U, 4, handle.AddrOfPinnedObject(), stride)) {
					Mat img2 = new Mat(size, DepthType.Cv8U, 3);
					CvInvoke.CvtColor(img, img2, ColorConversion.Bgra2Bgr);
					return img2;
				}
			}
			finally {
				if (handle.IsAllocated) {
					handle.Free();
				}
			}
		}

		public static Bitmap CopyToBitmap(this Mat mat) {
			Type colorType;
			switch (mat.NumberOfChannels) {
				case 1: colorType = typeof(Gray); break;
				case 3: colorType = typeof(Bgr);  break;
				case 4: colorType = typeof(Bgra); break;
				default: throw new Exception("Unknown color type");
			}

			return CvInvoke.RawDataToBitmap(mat.DataPointer, mat.Step, mat.Size, colorType, mat.NumberOfChannels, CvInvoke.GetDepthType(mat.Depth), false);
		}
	}
}
