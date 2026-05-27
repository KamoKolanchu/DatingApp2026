import { Component, ElementRef, inject, ViewChild } from '@angular/core';
import { BlockReasonModalService } from '../../core/services/block-reason-modal-service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-block-reason-modal',
  imports: [FormsModule],
  templateUrl: './block-reason-modal.html',
  styleUrl: './block-reason-modal.css',
})
export class BlockReasonModal {
  @ViewChild('modalRef') modalRef!: ElementRef<HTMLDialogElement>;
  title = '';
  reason = '';
  private resolver: ((result: string | null) => void) | null = null;

  constructor(){
  const service = inject(BlockReasonModalService);
  console.log('BlockReasonModal constructor - registering');
  service.register(this);
  console.log('BlockReasonModal registered');
  }

  open(memberName: string, reason:string): Promise<string | null> {
    this.title = `Block ${memberName}?`;
    this.reason = reason;
    this.modalRef.nativeElement.showModal();
    return new Promise(resolve => this.resolver = resolve);
  }

  confirm(){
     this.modalRef.nativeElement.close();
    this.resolver?.(this.reason || null);
    this.resolver = null;
  }

  cancel(){
    this.modalRef.nativeElement.close();
    this.resolver?.("");
    this.resolver = null;
  }
}
