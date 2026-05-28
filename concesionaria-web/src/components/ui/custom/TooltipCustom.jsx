import React, { useState, useRef } from "react";
import { createPortal } from "react-dom";

export function Tooltip({ children, text }) {
  const [show, setShow] = useState(false);
  const [coords, setCoords] = useState({ top: 0, left: 0 });
  const triggerRef = useRef(null);

  const handleMouseEnter = () => {
    if (triggerRef.current) {
      const rect = triggerRef.current.getBoundingClientRect();
      setCoords({
        top: rect.top,
        left: rect.left + rect.width / 2,
      });
    }
    setShow(true);
  };

  return (
    <>
      <div 
        ref={triggerRef}
        className="inline-block"
        onMouseEnter={handleMouseEnter} 
        onMouseLeave={() => setShow(false)}
      >
        {children}
      </div>

      {show && createPortal(
        <div 
          className="fixed z-[9999] pointer-events-none"
          style={{ 
            top: `${coords.top - 8}px`, 
            left: `${coords.left}px`,
            transform: 'translate(-50%, -100%)' 
          }}
        >
          <div className="text-white text-[11px] font-bold px-3 py-1.5 rounded-md shadow-2xl whitespace-nowrap animate-in fade-in zoom-in duration-150 border"
            style={{ 
              backgroundColor: 'hsl(var(--nav-bg))',
              borderColor: 'hsl(var(--nav-bg) / 0.5)' 
            }}>
            {text}
            <div 
              className="absolute top-full left-1/2 -translate-x-1/2 border-[5px] border-transparent"
              style={{ 
                borderTopColor: 'hsl(var(--nav-bg))' 
              }}
            />
          </div>
        </div>,
        document.body
      )}
    </>
  );
}