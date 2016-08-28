var walkies = walkies || {};

walkies.app = function( options )
{
    var that = this;

    var defaults =
    {
    }

    this.options = $.extend( {}, defaults, options );


    /* ---------------------------------------------------------------------------------------------
     * initialises the app
     */
    function init()
    {
        $( window ).scroll( onScroll );
    }

    /* ---------------------------------------------------------------------------------------------
     * occurs when the page scrolls
     */
    function onScroll()
    {
        var pos = $( window ).scrollTop();
        $( 'body' ).toggleClass( 'affixed', pos > 0 || window.location.pathname != '/' );
    }

    init();

};

