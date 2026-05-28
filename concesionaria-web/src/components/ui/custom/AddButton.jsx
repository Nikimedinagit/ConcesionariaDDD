import { useEffect } from "react";

import { Plus } from "lucide-react";
import { Button } from "@/components/ui/button";

const AddButton = ({
  children,
  onClick,
  className = "",
}) => {

  useEffect(() => {

    const handleKeyDown = (e) => {

      if (e.key === "Insert") {
        e.preventDefault();

        if (onClick) {
          onClick();
        }
      }
    };

    window.addEventListener("keydown", handleKeyDown);

    return () => {
      window.removeEventListener("keydown", handleKeyDown);
    };

  }, [onClick]);

  return (
    <Button
      onClick={onClick}
      className={`
        bg-[hsl(var(--nav-bg))]
        hover:opacity-90
        text-white
        shadow-sm
        font-medium
        transition-all duration-200
        ${className}
      `}
    >
      <Plus className="mr-2 h-4 w-4" />

      {children}
    </Button>
  );
};

export default AddButton;