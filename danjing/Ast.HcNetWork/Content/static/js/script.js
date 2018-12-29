var SEPARATION = 100;
var AMOUNTX = 50;
var AMOUNTY = 50;
var container, camera, scene, canvas, canvasWidth, canvasHeight;
var particles, particle, count = 0, mouseX = 0, mouseY = 0;

function initThreePartices(element) {
  container = element;
  canvasWidth = $(container).width();
  canvasHeight = $(container).height();

  camera = new THREE.PerspectiveCamera(75, canvasWidth / canvasHeight, 1, 10000);
  camera.position.z = 1000;
  scene = new THREE.Scene();
  particles = [];
  var PI2 = Math.PI * 2;
  var material = new THREE.ParticleCanvasMaterial({
    color: 0xffffff,
    program: function(context) {
      context.beginPath();
      context.arc(0, 0, 2, 0, PI2, true);
      context.fill();
    }
  });
  var i = 0;
  for (var ix = 0; ix < AMOUNTX; ix++) {
    for (var iy = 0; iy < AMOUNTY; iy++) {
      particle = particles[i++] = new THREE.Particle(material);
      particle.position.x = ix * SEPARATION - ((AMOUNTX * SEPARATION) / 2);
      particle.position.z = iy * SEPARATION - ((AMOUNTY * SEPARATION) / 2);
      scene.add(particle);
    }
  }
  canvas = new THREE.CanvasRenderer();
  canvas.setSize(canvasWidth, canvasHeight);
  element.appendChild(canvas.domElement);
  element.addEventListener('mousemove', onDocumentMouseMove, false);
  window.addEventListener('resize', onWindowResize, false);
}
function initThreeAnimate() {
  requestAnimationFrame(initThreeAnimate);
  initThreeRender();
}
function initThreeRender() {
  camera.position.x += (mouseX - camera.position.x) * .05;
  camera.position.y += (-mouseY - camera.position.y) * .05;
  camera.lookAt(scene.position);
  var i = 0;
  for (var ix = 0; ix < AMOUNTX; ix++) {
    for (var iy = 0; iy < AMOUNTY; iy++) {
      particle = particles[i++];
      particle.position.y = (Math.sin((ix + count) * 0.3) * 50) + (Math.sin((iy + count) * 0.5) * 50) - 300;
      particle.scale.x = particle.scale.y = (Math.sin((ix + count) * 0.3) + 1) * 1 + (Math.sin((iy + count) * 0.5) + 1) * 1;
    }
  }
  canvas.render(scene, camera);
  count += 0.1;
}
function onWindowResize() {
  canvasWidth = $(container).width();
  canvasHeight = $(container).height();
  camera.aspect = canvasWidth / canvasHeight;
  camera.updateProjectionMatrix();
  canvas.setSize(canvasWidth, canvasHeight);
}
function onDocumentMouseMove(event) {
  mouseX = event.clientX - canvasWidth/2;
  mouseY = event.clientY - canvasHeight/2;
}

// 控制内容点划线内容
var particlesConfig = {
  "particles": {
    "number": {
      "value": 50,
      "density": {
        "enable": true,
        "value_area": 1000
      }
    },
    "color": {
      "value": "#fff"
    },
    "shape": {
      "type": "circle",
      "stroke": {
        "width": 0,
        "color": "random" // shape
      },
      "polygon": {
        "nb_sides": 3
      },
      "image": {
        "src": "img/github.svg",
        "width": 100,
        "height": 100
      }
    },
    "opacity": {
      "value": 0.5,
      "random": false,
      "anim": {
        "enable": false,
        "speed": 1,
        "opacity_min": 0.1,
        "sync": false
      }
    },
    "size": {
      "value": 3,
      "random": true,
      "anim": {
        "enable": true,
        "speed": 40,
        "size_min": 0.1,
        "sync": false
      }
    },
    "line_linked": {
      "enable": true,
      "distance": 150,
      "color": "#ffffff", //link
      "opacity": 0.4,
      "width": 1
    },
    "move": {
      "enable": true,
      "speed": 6,
      "direction": "none",
      "random": false,
      "straight": false,
      "out_mode": "bounce",
      "bounce": false,
      "attract": {
        "enable": false,
        "rotateX": 400,
        "rotateY": 1200
      }
    }
  },
  "interactivity": {
    "detect_on": "canvas",
    "events": {
      "onhover": {
        "enable": true,
        "mode": "grab"
      },
      "onclick": {
        "enable": true,
        "mode": "repulse"
      },
      "resize": true
    },
    "modes": {
      "grab": {
        "distance": 400,
        "line_linked": {
          "opacity": 1
        }
      },
      "bubble": {
        "distance": 100,
        "size": 50,
        "duration": 2.2,
        "opacity": 1,
        "speed": 3
      },
      "repulse": {
        "distance": 100,
        "duration": 0.4
      },
      "push": {
        "particles_nb": 4
      },
      "remove": {
        "particles_nb": 2
      }
    }
  },
  "retina_detect": true
};




jQuery(document).ready(function($) {
  if ($.isFunction($.fn.lazyload)) {
    $("img.lazy").lazyload({effect: "fadeIn"});
  }

  if ($('#particles').length !== 0) {
    particlesJS('particles', particlesConfig);
  }


  // init banner bg
  if ($('#bannerParticles').length !== 0) {
    initThreePartices($('#bannerParticles').get(0));
    initThreeAnimate();
  }


  // scroll to top
  var $body = $(document.body);;
  var $fixedMenu = $('#fixedMenu');
  $(window).scroll(function() {
    var scrollHeight = $(document).height();
    var scrollTop = $(window).scrollTop();
    // var $footerHeight = $('.page-footer').outerHeight(true);
    var $windowHeight = $(window).innerHeight();
    scrollTop > 50 ? $("#fixedMenu").fadeIn(200).css("display", "block") : $("#fixedMenu").fadeOut(200);
    // $bottomTools.css("bottom", scrollHeight - scrollTop - $footerHeight > $windowHeight ? 40 : $windowHeight + scrollTop + $footerHeight + 40 - scrollHeight);
  });
  $('#scrollToUp').click(function(e) {
    e.preventDefault();
    $('html,body').animate({ scrollTop: 0 });
  });

  $('#menuNav').on('click', function(event) {
    event.preventDefault();
    var $nav = $('.header-navs');
    if($nav.is(':visible')){
      $nav.slideUp();
    } else {
      $nav.slideDown();
    }
  });
  // case list hover
  var caseBlocks = $('.case').find('.block-img');
  caseBlocks.each(function(index, el) {
    var $this = $(this);
    var $thisImg = $this.find('img');
    var halfHeight = $this.height() / 2;
    var moveTop = 0;
    $this.on('mouseleave', function(event) {
      event.preventDefault();
      $thisImg.css({ marginTop: 0 });
    });
    $this.on('mouseenter mousemove', function(event) {
      event.preventDefault();
      moveTop = $this.height() - $thisImg.height();

      if (event.offsetY > halfHeight) {
        $thisImg.css({ marginTop: moveTop });
      } else {
        $thisImg.css({ marginTop: 0 });
      }
    });
  });

  function get5View() {
    var width = $(window).width();
    if(width <= 767) {
      return 3;
    } else if(width >= 768 && width <= 991) {
      return 4;
    } else {
      return 5;
    }
  }
  function get4View() {
    var width = $(window).width();
    if(width <= 767) {
      return 2;
    } else if(width >= 768 && width <= 991) {
      return 3;
    } else {
      return 4;
    }
  }

  // swiper about
  var companyAboutSwiper = new Swiper('#companyAbout', {
    pagination: '.swiper-pagination',
    nextButton: '.swiper-button-next',
    prevButton: '.swiper-button-prev',
    slidesPerView: get5View(),
    paginationClickable: true,
    spaceBetween: 30
  });

  // swiper culture
   var companyCultureSwiper = new Swiper('#companyCulture', {
    nextButton: '.swiper-button-next',
    prevButton: '.swiper-button-prev',
    slidesPerView: get4View(),
    paginationClickable: true,
    spaceBetween: 30
  });

  var companyTeamSwiper = Swiper('#companyTeam', {
    nextButton: '.swiper-button-next',
    prevButton: '.swiper-button-prev',
    slidesPerView: get4View(),
    paginationClickable: true,
    spaceBetween: 30
  });

  var indexIntroSwiper = Swiper('#indexIntro', {
    nextButton: '.swiper-button-next',
    prevButton: '.swiper-button-prev',
    slidesPerView: get4View(),
    paginationClickable: true,
    spaceBetween: 30
  });

  $('[data-toggle="tab"]').on('shown.bs.tab', function(e) {
    e.target // newly activated tab
    e.relatedTarget // previous active tab
    companyCultureSwiper.update(true);
    companyTeamSwiper.update(true);
  });
});
