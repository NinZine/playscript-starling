// =================================================================================================
//
//	Starling Framework
//	Copyright 2011 Gamua OG. All Rights Reserved.
//
//	This program is free software. You can redistribute and/or modify it
//	in accordance with the terms of the accompanying license agreement.
//
// =================================================================================================

package starling.utils
{
    // TODO: add number formatting options
    
    /** Formats a String in .Net-style, with curly braces ("{0}"). Does not support any 
     *  number formatting options yet. */
    public function formatString(format:String, ...args):String
    {
        var i:int = 0;
        var re = new RegExp("\\{"+i+"\\}", "g");
        for each (var arg:Object in args) {
            format = re.replace(format, arg.ToString());
            i++;
        }
        return format;
    }
}