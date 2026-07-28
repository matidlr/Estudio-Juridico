import { Injectable } from '@angular/core';
import jsPDF from 'jspdf';

export interface PdfOpciones {
  fuente: 'helvetica' | 'times' | 'courier';
  tamanoLetra: number;
  colorEncabezado: 'azul' | 'verde' | 'negro' | 'bordo';
  incluirAclaraciones: boolean;
  numerarPaginas: boolean;
  orientacion: 'portrait' | 'landscape';
  incluirEncabezado: boolean;
}

export const PDF_OPCIONES_DEFAULT: PdfOpciones = {
  fuente: 'helvetica',
  tamanoLetra: 9,
  colorEncabezado: 'azul',
  incluirAclaraciones: true,
  numerarPaginas: true,
  orientacion: 'portrait',
  incluirEncabezado: true
};

@Injectable({
  providedIn: 'root'
})
export class PdfService {

  private readonly MARGEN_IZQ = 20;
  private readonly MARGEN_DER = 190;
  private readonly MARGEN_SUP = 20;
  private readonly COLOR_PRIMARY = [30, 58, 95];
  private readonly COLOR_TEXTO = [71, 85, 105];
  private readonly COLOR_MUTED = [148, 163, 184];
  private readonly STORAGE_KEY = 'pdf_opciones';

  guardarOpciones(opciones: PdfOpciones) {
    localStorage.setItem(this.STORAGE_KEY, JSON.stringify(opciones));
  }

  cargarOpciones(): PdfOpciones {
    const guardadas = localStorage.getItem(this.STORAGE_KEY);
    if (guardadas) {
      return JSON.parse(guardadas);
    }
    return PDF_OPCIONES_DEFAULT;
  }

 exportarExpediente(caso: any, fojas: any[], opciones: PdfOpciones = PDF_OPCIONES_DEFAULT) {
  const colores: Record<string, [number, number, number]> = {
    azul: [30, 58, 95],
    verde: [22, 163, 74],
    negro: [30, 30, 30],
    bordo: [127, 29, 29]
  };

  const colorPrimario = colores[opciones.colorEncabezado];
  const doc = new jsPDF({ orientation: opciones.orientacion });
  let y = this.MARGEN_SUP;

 // ── ENCABEZADO ──────────────────────────────────
if (opciones.incluirEncabezado) {
  doc.setFillColor(...colorPrimario);
  doc.rect(0, 0, 210, 40, 'F');

  doc.setTextColor(255, 255, 255);
  doc.setFontSize(18);
  doc.setFont(opciones.fuente, 'bold');
  doc.text('Estudio Juridico', this.MARGEN_IZQ, 16);

  doc.setFontSize(10);
  doc.setFont(opciones.fuente, 'normal');
  doc.text('Sistema de Gestion Legal', this.MARGEN_IZQ, 24);

  doc.setFontSize(9);
  doc.text(`Exportado: ${new Date().toLocaleDateString('es-AR')}`, this.MARGEN_DER, 24, { align: 'right' });

  y = 50;
} else {
  y = 20;
}

  doc.setTextColor(255, 255, 255);
  doc.setFontSize(18);
  doc.setFont(opciones.fuente, 'bold');
  doc.text('Estudio Juridico', this.MARGEN_IZQ, 16);

  doc.setFontSize(10);
  doc.setFont(opciones.fuente, 'normal');
  doc.text('Sistema de Gestion Legal', this.MARGEN_IZQ, 24);

  doc.setFontSize(9);
  doc.text(`Exportado: ${new Date().toLocaleDateString('es-AR')}`, this.MARGEN_DER, 24, { align: 'right' });

  y = 50;

  // ── DATOS DE LA CAUSA ───────────────────────────
  doc.setTextColor(...colorPrimario);
  doc.setFontSize(14);
  doc.setFont(opciones.fuente, 'bold');
  doc.text(caso.caratula, this.MARGEN_IZQ, y);
  y += 8;

  doc.setDrawColor(...colorPrimario);
  doc.setLineWidth(0.5);
  doc.line(this.MARGEN_IZQ, y, this.MARGEN_DER, y);
  y += 6;

  doc.setFontSize(9);
  doc.setFont(opciones.fuente, 'normal');
  doc.setTextColor(...this.COLOR_TEXTO as [number, number, number]);

  const datosLinea1 = [
    caso.nroExpediente ? `Expediente: ${caso.nroExpediente}` : '',
    caso.juzgado ? `Juzgado: ${caso.juzgado}` : '',
    caso.tipo ? `Tipo: ${caso.tipo}` : ''
  ].filter(Boolean).join('   |   ');

  const datosLinea2 = [
    caso.estado ? `Estado: ${caso.estado}` : '',
    caso.etapa ? `Etapa: ${caso.etapa}` : '',
    caso.proceso ? `Proceso: ${caso.proceso}` : ''
  ].filter(Boolean).join('   |   ');

  doc.text(datosLinea1, this.MARGEN_IZQ, y);
  y += 6;
  doc.text(datosLinea2, this.MARGEN_IZQ, y);
  y += 12;

  // ── FOJAS ───────────────────────────────────────
  fojas.forEach((foja, index) => {
    if (y > 250) {
      if (opciones.numerarPaginas) this.agregarPiePagina(doc, index);
      doc.addPage();
      y = 20;
    }

    doc.setFillColor(240, 245, 255);
    doc.rect(this.MARGEN_IZQ, y - 4, this.MARGEN_DER - this.MARGEN_IZQ, 10, 'F');

    doc.setTextColor(...colorPrimario);
    doc.setFontSize(10);
    doc.setFont(opciones.fuente, 'bold');
    const tituloFoja = foja.nroFoja ? `Foja ${foja.nroFoja}` : `Entrada ${index + 1}`;
    doc.text(tituloFoja, this.MARGEN_IZQ + 2, y + 3);

    doc.setFont(opciones.fuente, 'normal');
    doc.setFontSize(8);
    doc.setTextColor(...this.COLOR_MUTED as [number, number, number]);
    const fecha = new Date(foja.fecha).toLocaleDateString('es-AR');
    doc.text(fecha, this.MARGEN_DER, y + 3, { align: 'right' });

    y += 12;

    doc.setTextColor(...this.COLOR_TEXTO as [number, number, number]);
    doc.setFontSize(opciones.tamanoLetra);
    doc.setFont(opciones.fuente, 'normal');

    const lineas = doc.splitTextToSize(foja.contenido, this.MARGEN_DER - this.MARGEN_IZQ);
    lineas.forEach((linea: string) => {
      if (y > 270) {
        if (opciones.numerarPaginas) this.agregarPiePagina(doc, doc.getNumberOfPages());
        doc.addPage();
        y = 20;
      }
      doc.text(linea, this.MARGEN_IZQ, y);
      y += opciones.tamanoLetra * 0.6;
    });

    if (opciones.incluirAclaraciones && foja.aclaracionCliente) {
      y += 4;
      doc.setFillColor(255, 251, 235);
      const lineasAclaracion = doc.splitTextToSize(
        `Aclaracion: ${foja.aclaracionCliente}`,
        this.MARGEN_DER - this.MARGEN_IZQ - 8
      );
      const alturaAclaracion = lineasAclaracion.length * 5 + 8;

      doc.rect(this.MARGEN_IZQ, y - 2, this.MARGEN_DER - this.MARGEN_IZQ, alturaAclaracion, 'F');
      doc.setDrawColor(253, 230, 138);
      doc.rect(this.MARGEN_IZQ, y - 2, this.MARGEN_DER - this.MARGEN_IZQ, alturaAclaracion);

      doc.setTextColor(120, 80, 0);
      doc.setFontSize(8);
      lineasAclaracion.forEach((linea: string) => {
        doc.text(linea, this.MARGEN_IZQ + 4, y + 3);
        y += 5;
      });
      y += 6;
    }

    y += 8;

    doc.setDrawColor(226, 232, 240);
    doc.setLineWidth(0.2);
    doc.line(this.MARGEN_IZQ, y - 4, this.MARGEN_DER, y - 4);
  });

  if (opciones.numerarPaginas) this.agregarPiePagina(doc, doc.getNumberOfPages());

  const nombreArchivo = `Expediente_${caso.caratula.replace(/[^a-zA-Z0-9]/g, '_')}.pdf`;
  doc.save(nombreArchivo);
}

  private agregarPiePagina(doc: jsPDF, pagina: number) {
    const totalPaginas = doc.getNumberOfPages();
    doc.setPage(pagina);
    doc.setDrawColor(226, 232, 240);
    doc.setLineWidth(0.3);
    doc.line(20, 285, 190, 285);

    doc.setFontSize(8);
    doc.setTextColor(148, 163, 184);
    doc.setFont('helvetica', 'normal');
    doc.text('Estudio Jurídico — Documento confidencial', 20, 290);
    doc.text(`Página ${pagina} de ${totalPaginas}`, 190, 290, { align: 'right' });
  }

  
}