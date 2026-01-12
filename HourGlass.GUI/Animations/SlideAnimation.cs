namespace Hourglass.GUI.Animations;

using System;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Media;
using Avalonia.Styling;


public class SlideAnimation : PageSlide {

	public SlideAnimation(TimeSpan duration, SlideAxis orientation = SlideAxis.Horizontal, double? depth = null) : base(duration, orientation) {
		Depth = depth;
	}

	/// <summary>
	///  Defines the depth of the 3D Effect. If null, depth will be calculated automatically from the width or height
	///  of the common parent of the visual being rotated.
	/// </summary>
	public double? Depth { get; set; }

	/// <summary>
	///  Creates a new instance of the <see cref="Rotate3DTransition"/>
	/// </summary>
	public SlideAnimation() { }

	/// <inheritdoc />
	public override async Task Start(Visual? @from, Visual? to, bool forward, CancellationToken cancellationToken)
	{
		if (cancellationToken.IsCancellationRequested)
		{
			return;
		}

		var tasks = new Task[from != null && to != null ? 2 : 1];
		var parent = GetVisualParent(from, to);

		KeyFrame CreateKeyFrame(double cue, double opacity, int zIndex, bool isVisible = true) =>
			new()
			{
				Setters =
				{
					new Setter { Property = Visual.OpacityProperty, Value = opacity },
					new Setter { Property = Visual.IsVisibleProperty, Value = isVisible },
					new Setter { Property = Visual.RenderTransformProperty, Value=Transform.Parse("TranslateX(100px)")}
				},
				Cue = new Cue(cue)
			};

		if (from != null)
		{
			var animation = new Animation
			{
				Easing = SlideOutEasing,
				Duration = Duration,
				FillMode = FillMode.Forward,
				Children =
				{
					CreateKeyFrame(0d, 1.0d, 1),
					CreateKeyFrame(0.5d, 0.5, 1),
					CreateKeyFrame(1d, 0.0d, 2)
				}
			};

			tasks[0] = animation.RunAsync(from, cancellationToken);
		}

		if (to != null)
		{
			to.IsVisible = true;
			var animation = new Animation
			{
				Easing = SlideInEasing,
				Duration = Duration,
				FillMode = FillMode.Forward,
				Children =
				{
					CreateKeyFrame(0d, 0.0d, 1),
					CreateKeyFrame(0.5d, 0.5, 1),
					CreateKeyFrame(1d, 1.0d, 2)
				}
			};

			tasks[from != null ? 1 : 0] = animation.RunAsync(to, cancellationToken);
		}

		await Task.WhenAll(tasks);

		if (!cancellationToken.IsCancellationRequested)
		{
			if (to != null)
			{
				to.ZIndex = 2;
			}

			if (from != null)
			{
				from.IsVisible = false;
				from.ZIndex = 1;
			}
		}
	}
}