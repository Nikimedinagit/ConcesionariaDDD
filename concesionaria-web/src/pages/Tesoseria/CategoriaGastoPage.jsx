import { useState } from "react"; // <--- ESTA LÍNEA ES LA CLAVE
import PageHeader from "@/components/ui/custom/PageHeader";
import AddButton from "@/components/ui/custom/AddButton";
import CategoriaGastoTable from "@/components/tables/CategoriaGastoTable";
import { Wallet } from "lucide-react";
import { useCategorias } from "@/hooks/useCategorias";

export const CategoriaGastoPage = () => {
  const [tipo, setTipo] = useState('activas'); 
  const { data, loading } = useCategorias(tipo);

  return (
    <div className="min-h-screen">
      <PageHeader title="Categorías de Gasto" icon={Wallet}>
        <AddButton>Nueva Categoría</AddButton>
      </PageHeader>

      {loading ? (
        <div className="h-64 flex items-center justify-center">Cargando...</div>
      ) : (
        <CategoriaGastoTable 
          data={data} 
          tipo={tipo}           
          onToggle={setTipo}    
        />
      )}
    </div>
  );
};