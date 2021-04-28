(function($) {
  "use strict";

  // NAVIGATION
  var responsiveNav = $('#responsive-nav'),
    catToggle = $('#responsive-nav .category-nav .category-header'),
    catList = $('#responsive-nav .category-nav .category-list'),
    menuToggle = $('#responsive-nav .menu-nav .menu-header'),
    menuList = $('#responsive-nav .menu-nav .menu-list');

  catToggle.on('click', function() {
    menuList.removeClass('open');
    catList.toggleClass('open');
  });

  menuToggle.on('click', function() {
    catList.removeClass('open');
    menuList.toggleClass('open');
  });

  $(document).click(function(event) {
    if (!$(event.target).closest(responsiveNav).length) {
      if (responsiveNav.hasClass('open')) {
        responsiveNav.removeClass('open');
        $('#navigation').removeClass('shadow');
      } else {
        if ($(event.target).closest('.nav-toggle > button').length) {
          if (!menuList.hasClass('open') && !catList.hasClass('open')) {
            menuList.addClass('open');
          }
          $('#navigation').addClass('shadow');
          responsiveNav.addClass('open');
        }
      }
    }
  });

  // HOME SLICK
  $('#home-slick').slick({
    autoplay: true,
    infinite: true,
    speed: 300,
    arrows: true
  });

  // PRODUCTS SLICK
  $('#product-slick-1').slick({
    slidesToShow: 7,
    slidesToScroll: 7,
    autoplay: true,
    infinite: true,
    speed: 300,
    dots: true,
    arrows: false,
    appendDots: ".product-slick-dots-1",
      responsive: [
          {
              breakpoint: 2200,
              settings: {
                  slidesToShow: 6,
                  slidesToScroll: 6
              }
          },
          {
              breakpoint: 1800,
              settings: {
                  slidesToShow: 5,
                  slidesToScroll: 5
              }
          },
          {
              breakpoint: 1560,
              settings: {
                  slidesToShow: 5,
                  slidesToScroll: 5
              }
          },
        {
            breakpoint: 1100,
            settings: {
                slidesToShow: 3,
                slidesToScroll: 3
            }
        },
        {
            breakpoint: 830,
            settings: {
                slidesToShow: 2,
                slidesToScroll: 2
            }
        },
        {
            breakpoint: 480,
            settings: {
                dots: false,
                arrows: true,
                slidesToShow: 1,
                slidesToScroll: 1
            }
        }
    ]
  });

  $('#product-slick-2').slick({
      slidesToShow: 7,
      slidesToScroll: 7,
      autoplay: true,
      infinite: true,
      speed: 300,
      dots: true,
      arrows: false,
      appendDots: ".product-slick-dots-2",
      responsive: [
          {
              breakpoint: 2200,
              settings: {
                  slidesToShow: 6,
                  slidesToScroll: 6
              }
          },
          {
              breakpoint: 1800,
              settings: {
                  slidesToShow: 5,
                  slidesToScroll: 5
              }
          },
          {
              breakpoint: 1560,
              settings: {
                  slidesToShow: 5,
                  slidesToScroll: 5
              }
          },
          {
              breakpoint: 1100,
              settings: {
                  slidesToShow: 3,
                  slidesToScroll: 3
              }
          },
          {
              breakpoint: 830,
              settings: {
                  slidesToShow: 2,
                  slidesToScroll: 2
              }
          },
          {
              breakpoint: 480,
              settings: {
                  dots: false,
                  arrows: true,
                  slidesToShow: 1,
                  slidesToScroll: 1
              }
          }
      ]
  });

  $('#product-slick-3').slick({
      slidesToShow: 7,
      slidesToScroll: 7,
      autoplay: true,
      infinite: true,
      speed: 300,
      dots: true,
      arrows: false,
      appendDots: ".product-slick-dots-3",
      responsive: [
          {
              breakpoint: 2200,
              settings: {
                  slidesToShow: 6,
                  slidesToScroll: 6
              }
          },
          {
              breakpoint: 1800,
              settings: {
                  slidesToShow: 5,
                  slidesToScroll: 5
              }
          },
          {
              breakpoint: 1560,
              settings: {
                  slidesToShow: 5,
                  slidesToScroll: 5
              }
          },
          {
              breakpoint: 1100,
              settings: {
                  slidesToShow: 3,
                  slidesToScroll: 3
              }
          },
          {
              breakpoint: 830,
              settings: {
                  slidesToShow: 2,
                  slidesToScroll: 2
              }
          },
          {
              breakpoint: 480,
              settings: {
                  dots: false,
                  arrows: true,
                  slidesToShow: 1,
                  slidesToScroll: 1
              }
          }
      ]
  });

  $('#product-slick-details').slick({
      slidesToShow: 4,
      slidesToScroll: 4,
      autoplay: true,
      infinite: true,
      speed: 300,
      dots: true,
      arrows: false,
      appendDots: ".product-slick-dots-details",
      responsive: [
          {
              breakpoint: 2200,
              settings: {
                  slidesToShow: 4,
                  slidesToScroll: 4
              }
          },
          {
              breakpoint: 1800,
              settings: {
                  slidesToShow: 4,
                  slidesToScroll: 4
              }
          },
          {
              breakpoint: 1560,
              settings: {
                  slidesToShow: 4,
                  slidesToScroll: 4
              }
          },
          {
              breakpoint: 1100,
              settings: {
                  slidesToShow: 3,
                  slidesToScroll: 3
              }
          },
          {
              breakpoint: 830,
              settings: {
                  slidesToShow: 2,
                  slidesToScroll: 2
              }
          },
          {
              breakpoint: 480,
              settings: {
                  dots: false,
                  arrows: true,
                  slidesToShow: 1,
                  slidesToScroll: 1
              }
          }
      ]
  });

  // PRODUCT DETAILS SLICK
    $('#product-main-view').slick({
        infinite: true,
        speed: 300,
        dots: false,
        arrows: false,
        fade: true,
        asNavFor: '#product-view'
    });

  $('#product-view').slick({
    slidesToShow: 3,
    slidesToScroll: 1,
    arrows: true,
    infinite: true,
    centerMode: true,
    focusOnSelect: true,
    asNavFor: '#product-main-view'
  });

  $('#top-header-slick').slick({
      autoplay: true,
      infinite: true,
      speed: 1000,
      arrows: false,
      dots: false
  });
  // PRODUCT ZOOM
  $('#product-main-view .product-view').zoom({
      magnify: 0.2
  });


  // PRICE SLIDER
  var slider = document.getElementById('price-slider');
  if (slider) {
    noUiSlider.create(slider, {
      start: [1, 999],
      connect: true,
      tooltips: [true, true],
      format: {
        to: function(value) {
          return value.toFixed(2) + '$';
        },
        from: function(value) {
          return value;
        }
      },
      range: {
        'min': 1,
        'max': 999
      }
    });
  }

})(jQuery);
