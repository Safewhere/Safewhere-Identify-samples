(function ($, window, document, undefined) {

  var pluginName = "passtrength",
    defaults = {
      minChars: 4,
      tooltip: true,
      textWeak: 'Weak',
      textMedium: 'Medium',
      textStrong: 'Strong',
      textVeryStrong: 'Very Strong'
    };

  function Plugin(element, options) {
    this.element = element;
    this.$elem = $(this.element);
    this.options = $.extend({}, defaults, options);
    this._defaults = defaults;
    this._name = pluginName;
    _this = this;
    this.init();
  }

  Plugin.prototype = {
    init: function () {
      var _this = this,
        meter = jQuery('<div/>', { class: 'passtrengthMeter' }),
        tooltip = jQuery('<div/>', { class: 'tooltip' })

      meter.insertAfter(this.element);
      $(this.element).appendTo(meter);

      if (this.options.tooltip) {
        tooltip.appendTo(meter);
        var tooltipWidth = tooltip.outerWidth() / 2;
        tooltip.css('margin-left', -tooltipWidth)
      }

      this.$elem.bind('input', function (event) {
        value = $(this).val();
        _this.check(value);
      });

    },

    check: function (value) {

      var result = zxcvbn(value);
      var securePercentage = (result.score / 4) * 100;
      this.addStatus(securePercentage);

    },

    addStatus: function (percentage) {
      var status = '',
        text = '',
        meter = $(this.element).closest('.passtrengthMeter'),
        tooltip = meter.find('.tooltip');

      meter.attr('class', 'passtrengthMeter');

      if (percentage >= 25) {
        meter.attr('class', 'passtrengthMeter');
        status = 'weak';
        text = this.options.textWeak;
      }
      if (percentage >= 50) {
        meter.attr('class', 'passtrengthMeter');
        status = 'medium';
        text = this.options.textMedium;
      }
      if (percentage >= 75) {
        meter.attr('class', 'passtrengthMeter');
        status = 'strong';
        text = this.options.textStrong;
      }
      if (percentage >= 100) {
        meter.attr('class', 'passtrengthMeter');
        status = 'very-strong';
        text = this.options.textVeryStrong;
      }
      meter.addClass(status);
      if (this.options.tooltip) {
        tooltip.text(text);
      }

      if (this.options.calBackFunc) {
        if (status === 'strong' || status === 'very-strong') {
          this.options.calBackFunc(true);
        } else {
          this.options.calBackFunc(false);
        }
      }
    }
  }

  $.fn[pluginName] = function (options) {
    return this.each(function () {
      if (!$.data(this, "plugin_" + pluginName)) {
        $.data(this, "plugin_" + pluginName, new Plugin(this, options));
      }
    });
  };

})(jQuery, window, document);
