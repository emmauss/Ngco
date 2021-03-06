using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PrettyPrinter;

namespace Ngco {
	public class Style {
		public readonly List<Style> Parents = new List<Style>();
		public readonly List<ISelector> Selectors = new List<ISelector>();

		public Style(string selector = "") {
			foreach(var match in Regex.Matches(selector, @"(\.[^.#> ]+|#[^.#> ]+|[^.#> ]+|>)")) {
				var sel = match.ToString();
				if(sel[0] == '#')
					Selectors.Add(new IdSelector { Id = sel.Substring(1) });
				else if(sel[0] == '.')
					Selectors.Add(new ClassSelector { Class = sel.Substring(1) });
				else if(sel == ">")
					Selectors.Add(new DescendentSelector());
				else
					Selectors.Add(new WidgetSelector { Class = sel });
			}
		}

		public bool Match(BaseWidget widget) =>
			((IEnumerable<ISelector>) Selectors).Reverse().All(sel => (widget = sel.Match(widget)) != null);

		Color _TextColor;
		public Color TextColor {
			get {
				if(_TextColor != null) return _TextColor;
				foreach(var style in Parents) {
					var val = style.TextColor;
					if(val != null) return val;
				}
				return Parents.Count != 0 ? null : Context.Instance.BaseStyle._TextColor;
			}
			set => _TextColor = value;
		}
		
		int? _TextSize;
		public int? TextSize {
			get {
				if(_TextSize != null) return _TextSize;
				foreach(var style in Parents) {
					var val = style.TextSize;
					if(val != null) return val;
				}
				return Parents.Count != 0 ? null : Context.Instance.BaseStyle._TextSize;
			}
			set => _TextSize = value;
		}
		
		string _FontFamily;
		public string FontFamily {
			get {
				if(_FontFamily != null) return _FontFamily;
				foreach(var style in Parents) {
					var val = style.FontFamily;
					if(val != null) return val;
				}
				return Parents.Count != 0 ? null : Context.Instance.BaseStyle._FontFamily;
			}
			set => _FontFamily = value;
		}
	}
}