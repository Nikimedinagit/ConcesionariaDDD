import { useEffect, useMemo, useState } from "react";
import { useLocation, useNavigate, useParams } from "react-router-dom";
import {
  Building2,
  CarFront,
  Gauge,
  Palette,
  RectangleHorizontal,
} from "lucide-react";
import PageHeader from "@/components/ui/custom/PageHeader";
import { BackButton } from "@/components/ui/custom/BackButton";
import VehiculoService from "@/services/Vehiculo/vehiculoService";

const estados = { 1: "Disponible", 2: "Vendido", 3: "Reservado", 4: "En servicio" };
const condiciones = { 1: "NUEVO", 2: "USADO", 3: "CONSIGNACIÓN" };

const formatNumber = (value) =>
  value == null ? "—" : new Intl.NumberFormat("es-AR").format(value);

const formatMoney = (value) =>
  value == null
    ? "—"
    : `$ ${new Intl.NumberFormat("en-US", {
        minimumFractionDigits: 2,
        maximumFractionDigits: 2,
      }).format(value)}`;

export function VehiculoDetallePage() {
  const navigate = useNavigate();
  const { id } = useParams();
  const { state } = useLocation();
  const storageKey = `vehiculo-detalle:${id}`;
  const vehiculo = useMemo(() => {
    if (state?.vehiculo) return state.vehiculo;

    try {
      return JSON.parse(sessionStorage.getItem(storageKey));
    } catch {
      return null;
    }
  }, [state, storageKey]);
  const [imagenes, setImagenes] = useState([]);
  const [imagenActiva, setImagenActiva] = useState(0);
  const [loadingImages, setLoadingImages] = useState(Boolean(vehiculo));
  const [imageError, setImageError] = useState("");

  useEffect(() => {
    if (state?.vehiculo) {
      sessionStorage.setItem(storageKey, JSON.stringify(state.vehiculo));
    }
  }, [state, storageKey]);

  useEffect(() => {
    if (!vehiculo) return;

    let active = true;
    setLoadingImages(true);

    VehiculoService.getImagenes(id)
      .then((data) => {
        if (!active) return;
        setImagenes(data);
        const principalIndex = data.findIndex((image) => image.esPrincipal);
        setImagenActiva(principalIndex >= 0 ? principalIndex : 0);
      })
      .catch(() => {
        if (active) setImageError("No se pudieron cargar las imágenes.");
      })
      .finally(() => {
        if (active) setLoadingImages(false);
      });

    return () => {
      active = false;
    };
  }, [id, vehiculo]);

  if (!vehiculo) {
    return (
      <div>
        <PageHeader title="Vehículo no encontrado" icon={CarFront} />
        <div className="rounded-xl border border-slate-200 bg-white p-6 text-slate-600 shadow-sm">
          No se encontró la información del vehículo.
          <BackButton className="mt-4" onClick={() => navigate("/layout/vehiculos")}>
            Volver a vehículos
          </BackButton>
        </div>
      </div>
    );
  }

  return (
    <div>
      <PageHeader title="Detalle del vehículo" icon={CarFront}>
        <BackButton onClick={() => navigate("/layout/vehiculos")} />
      </PageHeader>

      <article className="rounded-2xl border border-slate-200 bg-white p-4 shadow-[0_8px_30px_rgba(15,23,42,0.08)] sm:p-5 lg:p-6">
        <div className="grid gap-6 lg:grid-cols-[minmax(0,1.35fr)_minmax(360px,0.65fr)]">
          <section className="min-w-0 lg:relative lg:min-h-0">
            <div className="flex flex-col-reverse gap-3 sm:flex-row lg:absolute lg:inset-0 lg:min-h-0">
              {imagenes.length > 1 && (
                <div className="flex shrink-0 gap-2 overflow-hidden p-0.5 sm:h-[360px] sm:w-[74px] sm:flex-col lg:h-auto lg:self-stretch">
                  {imagenes.slice(0, 5).map((image, index) => (
                    <button
                      key={image.imagenId}
                      type="button"
                      onClick={() => setImagenActiva(index)}
                      className={`h-16 w-16 shrink-0 overflow-hidden rounded-lg bg-white ring-2 transition-all ${
                        imagenActiva === index
                          ? "ring-[hsl(var(--nav-bg))]"
                          : "ring-slate-200 hover:ring-slate-400"
                      }`}
                    >
                      <img src={image.url} alt={`Imagen ${index + 1}`} className="h-full w-full object-cover" />
                    </button>
                  ))}

                  {imagenes.length > 5 && (
                    <button
                      type="button"
                      onClick={() =>
                        setImagenActiva((current) =>
                          current >= 5
                            ? 5 + ((current - 5 + 1) % (imagenes.length - 5))
                            : 5,
                        )
                      }
                      className={`relative h-16 w-16 shrink-0 overflow-hidden rounded-lg ring-2 transition-all ${
                        imagenActiva >= 5
                          ? "ring-[hsl(var(--nav-bg))]"
                          : "ring-slate-200 hover:ring-slate-400"
                      }`}
                      aria-label={`Ver ${imagenes.length - 5} imágenes adicionales`}
                    >
                      <img
                        src={imagenes[imagenActiva >= 5 ? imagenActiva : 5]?.url}
                        alt="Más imágenes"
                        className="h-full w-full object-cover"
                      />
                      <span className="absolute inset-0 flex items-center justify-center bg-slate-950/65 text-sm font-black text-white backdrop-blur-[1px]">
                        +{imagenes.length - 5}
                      </span>
                    </button>
                  )}
                </div>
              )}

              <div className="relative flex h-[280px] flex-1 items-center justify-center overflow-hidden rounded-xl bg-slate-50 ring-1 ring-slate-200 sm:h-[360px] lg:h-auto lg:min-h-0 lg:self-stretch">
                {loadingImages ? (
                  <span className="text-sm font-medium text-slate-500">Cargando imágenes...</span>
                ) : imagenes.length ? (
                  <img
                    src={imagenes[imagenActiva]?.url}
                    alt={`${vehiculo.marcaNombre} ${vehiculo.modeloNombre}`}
                    className="h-full w-full object-contain p-2"
                  />
                ) : (
                  <div className="flex flex-col items-center gap-2 text-slate-400">
                    <CarFront className="h-16 w-16" />
                    <span className="text-sm font-semibold">Sin imágenes</span>
                  </div>
                )}

                {imagenes.length > 0 && (
                  <span className="absolute bottom-3 right-3 rounded-full bg-slate-950/75 px-2.5 py-1 text-xs font-bold text-white backdrop-blur-sm">
                    {imagenActiva + 1} / {imagenes.length}
                  </span>
                )}
              </div>
            </div>

            {imageError && <p className="mt-3 text-sm font-medium text-red-600">{imageError}</p>}
          </section>

          <aside className="min-w-0 lg:border-l lg:border-slate-200 lg:pl-6">
            <div className="flex items-center justify-between gap-3">
              <p className="text-sm text-slate-500">
                {condiciones[vehiculo.condicion] || "Vehículo"} · {vehiculo.anio}
              </p>
              <span className="rounded-full bg-emerald-50 px-3 py-1 text-[11px] font-black uppercase text-emerald-700 ring-1 ring-emerald-200">
                {estados[vehiculo.estado] || "Sin estado"}
              </span>
            </div>

            <h2 className="mt-3 text-2xl font-black uppercase leading-tight tracking-tight text-slate-950">
              {[vehiculo.marcaNombre, vehiculo.modeloNombre].filter(Boolean).join(" ")}
            </h2>
            <p className="mt-1 text-sm font-semibold uppercase text-slate-500">
              {vehiculo.version || "Sin versión"}
            </p>

            <div className="my-5 border-y border-slate-200 py-5">
              <p className="text-xs font-bold uppercase tracking-wide text-slate-500">Precio de venta</p>
              <p className="mt-1 text-3xl font-black tracking-tight text-emerald-700">
                {formatMoney(vehiculo.precioVenta)}
              </p>
              <p className="mt-2 text-xs font-semibold text-slate-500">
                PRECIO DE COMPRA: <strong className="text-slate-700">{formatMoney(vehiculo.precioCompra)}</strong>
              </p>
            </div>

            <h3 className="mb-3 text-sm font-black uppercase text-slate-800">Características principales</h3>
            <div className="grid grid-cols-2 gap-x-5 gap-y-3">
              <DetailItem icon={Gauge} label="Kilometraje" value={`${formatNumber(vehiculo.kilometraje)} km`} />
              <DetailItem icon={Palette} label="Color" value={vehiculo.color} />
              <DetailItem icon={RectangleHorizontal} label="Patente" value={vehiculo.patente} />
              <DetailItem icon={CarFront} label="Tipo" value={vehiculo.tipoVehiculoNombre} />
            </div>

            <div className="mt-4 flex items-center gap-3 rounded-xl bg-slate-50 p-3 ring-1 ring-slate-200">
              <div className="flex h-9 w-9 shrink-0 items-center justify-center rounded-full bg-white text-[hsl(var(--nav-bg))] shadow-sm ring-1 ring-slate-200">
                <Building2 className="h-4 w-4" />
              </div>
              <div className="min-w-0">
                <p className="text-[11px] font-bold uppercase tracking-wide text-slate-500">Publicado por</p>
                <p className="truncate text-sm font-black uppercase text-slate-900">{vehiculo.sucursalNombre || "Sin sucursal"}</p>
              </div>
            </div>
          </aside>
        </div>
      </article>
    </div>
  );
}

function DetailItem({ icon: Icon, label, value }) {
  return (
    <div className="min-w-0 border-b border-slate-100 pb-3">
      <div className="flex items-center gap-1.5 text-[11px] font-bold uppercase tracking-wide text-slate-500">
        <Icon className="h-3.5 w-3.5 shrink-0 text-[hsl(var(--nav-bg))]" />
        {label}
      </div>
      <p className="mt-1 truncate text-sm font-black uppercase text-slate-900">{value || "—"}</p>
    </div>
  );
}
