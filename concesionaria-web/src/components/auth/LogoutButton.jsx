import { useNavigate } from "react-router-dom";
import { LogOut } from "lucide-react";
import { DropdownMenuItem } from "@/components/ui/dropdown-menu";

export function LogoutButton({ className }) {
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem("token");
    navigate("/"); 
  };

  return (
    <DropdownMenuItem 
      onClick={handleLogout}
      className={className}
    >
      <LogOut className="h-4 w-4" /> Cerrar Sesión
    </DropdownMenuItem>
  );
}