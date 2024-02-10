
using VisualizeEmployeesData.Models;
using OxyPlot.Series;
using OxyPlot;
using SkiaSharp;
using Svg.Skia;

namespace VisualizeEmployeesData.Services
{
    public class EmployeeDataChartService
    {
        public void GeneratePieChartImage(List<EmployeeHours> employeeHours)
        {
            var model = new PlotModel { Title = "Employee Work Hours Visualized" };
            var pieSeries = new PieSeries { StrokeThickness = 2.0, InsideLabelPosition = 0.8, AngleSpan = 360, StartAngle = 0 };

            double totalHours = employeeHours.Sum(e => e.TotalHours);
            foreach (var employee in employeeHours)
            {
                double percentage = (employee.TotalHours / totalHours) * 100;
                pieSeries.Slices.Add(new PieSlice(employee.EmployeeName, percentage) { IsExploded = false });
            }

            model.Series.Add(pieSeries);

            var svgExporter = new SvgExporter { Width = 700, Height = 600 };
            using (var stream = File.Create("EmployeeWorkHoursPieChart.svg"))
            {
                svgExporter.Export(model, stream);
            }
            ConvertSvgToPng("EmployeeWorkHoursPieChart.svg", "EmployeeWorkHoursPieChart.png");


        }
        public void ConvertSvgToPng(string svgFilePath, string pngFilePath)
        {
            var svg = new SKSvg();
            svg.Load(svgFilePath);

            var width = svg.Picture.CullRect.Width;
            var height = svg.Picture.CullRect.Height;

            using var bitmap = new SKBitmap((int)width, (int)height);
            using var canvas = new SKCanvas(bitmap);

            canvas.Clear(SKColors.White);
            canvas.DrawPicture(svg.Picture);
            canvas.Flush();

            using var image = SKImage.FromBitmap(bitmap);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100); 
            using var stream = File.OpenWrite(pngFilePath);
            data.SaveTo(stream);
            File.Delete(svgFilePath);
        }
    }
}
